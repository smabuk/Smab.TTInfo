using HtmlAgilityPack;

namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
	public async Task<MatchCard?> GetMatchCard(TT365LeagueId leagueId, int matchCardId, TT365SeasonId? seasonId = null)
	{
		League? league = await GetLeague(leagueId);
		TT365SeasonId currentSeasonId = league?.CurrentSeasonId ?? throw new InvalidOperationException($"League with ID {leagueId} does not have a current season.");
		seasonId ??= currentSeasonId;
		if (seasonId is null) {
			// TODO: Log this error
			return null;
		}

		string filename = $@"{leagueId}_match_{matchCardId}.json";

		// Attempt to load fixtures from cache
		// If the requested season is the current season, use the default cache expiration; otherwise, use a long expiration for historical data
		MatchCard? matchCard = currentSeasonId == seasonId
			? await LoadAsync<MatchCard?>(leagueId, null, filename)
			: await LoadAsync<MatchCard?>(leagueId, null, filename, cacheHours: 10_000_000);

		if (matchCard is not null) { return matchCard; }

		string url = $"Results/MatchCard?matchId={matchCardId}";
		HtmlDocument? doc = await LoadAsync<HtmlDocument>(
			leagueId,
			url);

		if (string.IsNullOrWhiteSpace(doc?.Text)) { return matchCard; }

		if (doc.DocumentNode.SelectNodes("//div[contains(@class, 'tt-matchcard-teams')]") is null) { return matchCard; }

		string homeTeamName = doc.DocumentNode.SelectSingleNode("//span[contains(@class, 'tt-matchcard-team-home')]").InnerText.Trim();
		string awayTeamName = doc.DocumentNode.SelectSingleNode("//span[contains(@class, 'tt-matchcard-team-away')]").InnerText.Trim();

		HtmlNode meta = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'tt-matchcard-meta')]");
		int weekNo = meta.ChildNodes.Where(x => x.Name == "span" && !x.HasClass("tt-matchcard-meta-sep")).Select(x => x.InnerText.Trim()).Where(x => x.StartsWith("Match -")).Select(x => int.Parse(x.Replace("Match -", "").Trim())).FirstOrDefault();
		DateOnly matchDate = meta.ChildNodes.Where(x => x.Name == "span" && !x.HasClass("tt-matchcard-meta-sep")).Select(x => x.InnerText.Trim()).Where(x => DateOnly.TryParse(x, out _)).Select(x => DateOnly.Parse(x)).FirstOrDefault();
		string divName = meta.ChildNodes.Where(x => x.Name == "span" && !x.HasClass("tt-matchcard-meta-sep")).Select(x => x.InnerText.Trim()).Where(x => !DateOnly.TryParse(x, out _)).LastOrDefault() ?? "";
		int forHome = int.Parse(doc.DocumentNode.SelectSingleNode("//span[contains(@class, 'tt-matchcard-score-home')]").InnerText.Trim());
		int forAway = int.Parse(doc.DocumentNode.SelectSingleNode("//span[contains(@class, 'tt-matchcard-score-away')]").InnerText.Trim());
		string potm = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'tt-matchcard-potm')]")?.SelectSingleNode(".//a")?.InnerText.Trim() ?? "";

		matchCard = new(matchCardId, matchDate, homeTeamName, awayTeamName, divName, forHome, forAway);
		matchCard = matchCard with { PlayerOfTheMatch = potm };

		HtmlNode? matchCardTypeA = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'tt-matchcard-a')]");
		HtmlNode? matchCardTypeB = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'tt-matchcard-b')]");

		if (matchCardTypeA is not null) {
			matchCard = ParseMatchCardTypeA(doc, matchCard, leagueId, seasonId);
		} else if (matchCardTypeB is not null) {
			matchCard = ParseMatchCardTypeB(doc, matchCard, leagueId, seasonId);
		}

		if (matchCard.Sets is not []) {
			List<MatchPlayer> homePlayers = [.. matchCard.Sets.SelectMany(set => new[] { set.HomePlayer, set.HomeDoublesPartner }.Where(p => p is not null).Select(p => new MatchPlayer(p!.Name, p.PlayerId, 0, false))).DistinctBy(p => p.Id)];
			List<MatchPlayer> awayPlayers = [.. matchCard.Sets.SelectMany(set => new[] { set.AwayPlayer, set.AwayDoublesPartner }.Where(p => p is not null).Select(p => new MatchPlayer(p!.Name, p.PlayerId, 0, false))).DistinctBy(p => p.Id)];

			// we also need to count SetsWon now that we have everything fully parsed, as the TT365 site does not provide this information in the matchcard page
			for (int i = 0; i < homePlayers.Count; i++) {
				MatchPlayer player = homePlayers[i];
				double homePlayerSetsWon
					= matchCard.Sets.Where(set => !set.IsDoubles && set.HomePlayer.Name == player.Name).Sum(s => s.HomeWin ? 1 : 0)
					+ matchCard.Sets.Where(set => set.IsDoubles && (set.HomePlayer.Name == player.Name || set.HomeDoublesPartner?.Name == player.Name))
					.Sum(s => matchCardTypeB is not null
						? 0 :
						s.HomeWin ? 0.5: 0);
				homePlayers[i] = player with { SetsWon = homePlayerSetsWon };
			}

			for (int i = 0; i < awayPlayers.Count; i++) {
				MatchPlayer player = awayPlayers[i];
				double awayPlayerSetsWon = matchCard.Sets.Where(set => !set.IsDoubles && set.AwayPlayer.Name == player.Name).Sum(s => s.AwayWin ? 1 : 0)
					+ matchCard.Sets.Where(set => set.IsDoubles && (set.AwayPlayer.Name == player.Name || set.AwayDoublesPartner?.Name == player.Name))
					.Sum(s => matchCardTypeB is not null
						? 0 :
						s.HomeWin ? 0.5 : 0);
				awayPlayers[i] = player with { SetsWon = awayPlayerSetsWon };
			}

			matchCard = matchCard with
			{
				HomePlayers = homePlayers,
				AwayPlayers = awayPlayers,
				PlayerOfTheMatch = potm
			};
		}

		string jsonString = JsonSerializer.Serialize(matchCard);
		_ = SaveFileToCache(jsonString, filename);

		return matchCard;
	}

	private static MatchCard ParseMatchCardTypeA(HtmlDocument doc, MatchCard matchCard, TT365LeagueId leagueId, TT365SeasonId? seasonId)
	{
		HtmlNode setsTableBody = doc.DocumentNode.SelectSingleNode("//table/tbody");
		foreach (HtmlNode row in setsTableBody.SelectNodes("./tr") ?? EMPTY_NODE_COLLECTION) {
			HtmlNode homePlayerNode = row.SelectSingleNode("./td[contains(@class, 'tt-matchcard-col-player')][1]");
			HtmlNode awayPlayerNode = row.SelectSingleNode("./td[contains(@class, 'tt-matchcard-col-player')][2]");
			HtmlNode gamesNode = row.SelectSingleNode("./td[contains(@class, 'tt-matchcard-col-games')]");
			HtmlNode scoreNode = row.SelectSingleNode("./td[contains(@class, 'tt-matchcard-col-score')]");

			string homePlayerName = homePlayerNode?.SelectSingleNode(".//a")?.InnerText.Trim() ?? "";
			int homePlayerId = int.Parse(homePlayerNode?.SelectSingleNode(".//a")?.GetAttributeValue("href", "").Split("id=").LastOrDefault() ?? "0");
			string? homeDoublesPartnerName = homePlayerNode?.SelectNodes(".//a")?.ElementAtOrDefault(1)?.InnerText.Trim();
			int homeDoublesPartnerId = int.Parse(homePlayerNode?.SelectNodes(".//a")?.ElementAtOrDefault(1)?.GetAttributeValue("href", "").Split("id=").LastOrDefault() ?? "0");

			string awayPlayerName = awayPlayerNode?.SelectSingleNode(".//a")?.InnerText.Trim() ?? "";
			int awayPlayerId = int.Parse(awayPlayerNode?.SelectSingleNode(".//a")?.GetAttributeValue("href", "").Split("id=").LastOrDefault() ?? "0");
			string? awayDoublesPartnerName = awayPlayerNode?.SelectNodes(".//a")?.ElementAtOrDefault(1)?.InnerText.Trim();
			int awayDoublesPartnerId = int.Parse(awayPlayerNode?.SelectNodes(".//a")?.ElementAtOrDefault(1)?.GetAttributeValue("href", "").Split("id=").LastOrDefault() ?? "0");

			string games = string.Join(",", gamesNode?.InnerText.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? []);
			int homeScore = int.Parse(scoreNode?.SelectSingleNode("./span[1]")?.InnerText.Trim() ?? "0");
			int awayScore = int.Parse(scoreNode?.SelectSingleNode("./span[2]")?.InnerText.Trim() ?? "0");

			MatchSet set = new(matchCard.Sets.Count + 1, new Player() { Name = homePlayerName, PlayerId = homePlayerId }, new Player() { Name = awayPlayerName, PlayerId = awayPlayerId }, games, $"{homeScore}-{awayScore}", "");
			if (!string.IsNullOrWhiteSpace(homeDoublesPartnerName) && homeDoublesPartnerId > 0 && !string.IsNullOrWhiteSpace(awayDoublesPartnerName) && awayDoublesPartnerId > 0) {
				set = set with
				{
					HomeDoublesPartner = new Player() { Name = homeDoublesPartnerName, PlayerId = homeDoublesPartnerId },
					AwayDoublesPartner = new Player() { Name = awayDoublesPartnerName, PlayerId = awayDoublesPartnerId }
				};
			}

			matchCard.Sets.Add(set);
		}



		return matchCard;
	}

	private static MatchCard ParseMatchCardTypeB(HtmlDocument doc, MatchCard matchCard, TT365LeagueId leagueId, TT365SeasonId? seasonId)
	{
		HtmlNode setsTableHeader = doc.DocumentNode.SelectSingleNode("//table/thead");
		foreach (HtmlNode headerSpan in setsTableHeader.SelectNodes(".//th/span") ?? EMPTY_NODE_COLLECTION) { // Away players are in the header row
			string awayPlayerName = headerSpan?.SelectSingleNode(".//a")?.InnerText.Trim() ?? "";
			int awayPlayerId = int.Parse(headerSpan?.SelectSingleNode(".//a")?.GetAttributeValue("href", "").Split("id=").LastOrDefault() ?? "0");
			matchCard.AwayPlayers.Add(new MatchPlayer(awayPlayerName, awayPlayerId, 0, false));
		}

		HtmlNode setsTableBody = doc.DocumentNode.SelectSingleNode("//table/tbody");
		// home players are in the body rows, and each row contains the games played against each away player
		foreach (HtmlNode row in setsTableBody.SelectNodes("./tr") ?? EMPTY_NODE_COLLECTION) {
			HtmlNode homePlayerNode = row.SelectSingleNode("./th[contains(@class, 'tt-matchcard-matrix-rowheader')]");
			string homePlayerName = homePlayerNode?.SelectSingleNode(".//a")?.InnerText.Trim() ?? "";
			int homePlayerId = int.Parse(homePlayerNode?.SelectSingleNode(".//a")?.GetAttributeValue("href", "").Split("id=").LastOrDefault() ?? "0");
			matchCard.HomePlayers.Add(new MatchPlayer(homePlayerName, homePlayerId, 0, false));
		}

		int[] setOrder = [1, 10, 5, 4, 2, 8, 9, 6, 3, 7]; // default set order
		int setIndex = 0;
		foreach (HtmlNode cell in setsTableBody.SelectNodes(".//tr/td[contains(@class, 'tt-matchcard-matrix-cell')]") ?? EMPTY_NODE_COLLECTION) {
			int setNo = setOrder[setIndex];
			MatchPlayer homeMatchPlayer = matchCard.HomePlayers[setIndex / matchCard.AwayPlayers.Count];
			MatchPlayer awayMatchPlayer = matchCard.AwayPlayers[setIndex % matchCard.AwayPlayers.Count];
			string scores = string.Join(",", cell.SelectNodes(".//span[contains(@class, 'tt-matchcard-matrix-game')]")?.Select(x => x.InnerText.Trim()) ?? []);
			string result = cell.SelectSingleNode(".//div[contains(@class, 'tt-matchcard-matrix-result')]")?.InnerText.Replace(" ", "").Trim() ?? "";
			string? resultReason = cell.SelectSingleNode(".//div[contains(@class, 'tt-matchcard-scratched')]")?.InnerText.Trim();

			MatchSet set = new(
				setNo,
				new Player() { PlayerId = homeMatchPlayer.Id, Name = homeMatchPlayer.Name },
				new Player() { PlayerId = awayMatchPlayer.Id, Name = awayMatchPlayer.Name },
				scores,
				result,
				resultReason);

			setIndex++;
			matchCard.Sets.Add(set);
		}

		// The doubles set is in the footer row, and the players are in the doubles label div
		//                           <tr class="tt-matchcard-doubles-row">
		//                               <th class="tt-matchcard-matrix-rowheader" colspan="4">
		//                                   <div class="tt-matchcard-doubles-label">Doubles</div>
		//                                   <div class="tt-matchcard-doubles-players">
		//                                       <span>
		//                                               <span class=""><a href="/Reading/Results/Player?leagueName=Senior%202024-25&amp;playerName=Cris%20Reynolds&amp;id=374563" class="tt-player-link">Cris Reynolds</a></span>
		//&amp;                                                 <span class=""><a href="/Reading/Results/Player?leagueName=Senior%202024-25&amp;playerName=James%20Miller&amp;id=374562" class="tt-player-link">James Miller</a></span>
		//                                       </span>
		//                                       <span class="tt-matchcard-vs">vs</span>
		//                                       <span>
		//                                               <span class=""><a href="/Reading/Results/Player?leagueName=Senior%202024-25&amp;playerName=Mark%20Carless&amp;id=376997" class="tt-player-link">Mark Carless</a></span>
		//&amp;                                                 <span class=""><a href="/Reading/Results/Player?leagueName=Senior%202024-25&amp;playerName=Gopi%20Ponnapalli&amp;id=377035" class="tt-player-link">Gopi Ponnapalli</a></span>
		//                                       </span>
		//                                   </div>
		//                                   <div class="tt-matchcard-doubles-games tt-matchcard-games">8-11  11-6  9-11  11-5  11-3</div>
		//                               </th>
		//                               <td class="tt-matchcard-matrix-rowtotal">1 - 0</td>
		//                           </tr>
		//		HtmlNode doublesRow = doc.DocumentNode.SelectSingleNode("//tfoot/tr[contains(@class, 'tt-matchcard-doubles-row')]");

		Player homePlayer = new()
		{
			Name = doc.DocumentNode.SelectSingleNode("//tfoot/tr[contains(@class, 'tt-matchcard-doubles-row')]//div[contains(@class, 'tt-matchcard-doubles-players')]/span[1]/span[1]/a")?.InnerText.Trim() ?? "",
			PlayerId = int.Parse(doc.DocumentNode.SelectSingleNode("//tfoot/tr[contains(@class, 'tt-matchcard-doubles-row')]//div[contains(@class, 'tt-matchcard-doubles-players')]/span[1]/span[1]/a")?.GetAttributeValue("href", "").Split("id=").LastOrDefault() ?? "0")
		};

		Player homeDoublesPartner = new()
		{
			Name = doc.DocumentNode.SelectSingleNode("//tfoot/tr[contains(@class, 'tt-matchcard-doubles-row')]//div[contains(@class, 'tt-matchcard-doubles-players')]/span[1]/span[2]/a")?.InnerText.Trim() ?? "",
			PlayerId = int.Parse(doc.DocumentNode.SelectSingleNode("//tfoot/tr[contains(@class, 'tt-matchcard-doubles-row')]//div[contains(@class, 'tt-matchcard-doubles-players')]/span[1]/span[2]/a")?.GetAttributeValue("href", "").Split("id=").LastOrDefault() ?? "0")
		};

		Player awayPlayer = new()
		{
			Name = doc.DocumentNode.SelectSingleNode("//tfoot/tr[contains(@class, 'tt-matchcard-doubles-row')]//div[contains(@class, 'tt-matchcard-doubles-players')]/span[3]/span[1]/a")?.InnerText.Trim() ?? "",
			PlayerId = int.Parse(doc.DocumentNode.SelectSingleNode("//tfoot/tr[contains(@class, 'tt-matchcard-doubles-row')]//div[contains(@class, 'tt-matchcard-doubles-players')]/span[3]/span[1]/a")?.GetAttributeValue("href", "").Split("id=").LastOrDefault() ?? "0")
		};

		Player awayDoublesPartner = new()
		{
			Name = doc.DocumentNode.SelectSingleNode("//tfoot/tr[contains(@class, 'tt-matchcard-doubles-row')]//div[contains(@class, 'tt-matchcard-doubles-players')]/span[3]/span[2]/a")?.InnerText.Trim() ?? "",
			PlayerId = int.Parse(doc.DocumentNode.SelectSingleNode("//tfoot/tr[contains(@class, 'tt-matchcard-doubles-row')]//div[contains(@class, 'tt-matchcard-doubles-players')]/span[3]/span[2]/a")?.GetAttributeValue("href", "").Split("id=").LastOrDefault() ?? "0")
		};

		string doublesGames = string.Join(",", doc.DocumentNode.SelectSingleNode("//tfoot/tr[contains(@class, 'tt-matchcard-doubles-row')]//div[contains(@class, 'tt-matchcard-doubles-games')]")?.InnerText.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? []);
		string doublesResult = doc.DocumentNode.SelectSingleNode("//tfoot/tr[contains(@class, 'tt-matchcard-doubles-row')]//td[contains(@class, 'tt-matchcard-matrix-rowtotal')]")?.InnerText.Replace(" ", "").Trim() ?? "";
		matchCard.Sets.Add(new MatchSet(setOrder[setIndex], homePlayer, awayPlayer, doublesGames, doublesResult, "") with { HomeDoublesPartner = homeDoublesPartner, AwayDoublesPartner = awayDoublesPartner });

		return matchCard;
	}
}
