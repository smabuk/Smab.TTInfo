using HtmlAgilityPack;

using Smab.TTInfo.TT365.Models.TT365;

namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
	/// <summary>
	/// Retrieves a list of fixtures for a specified league and season.
	/// </summary>
	/// <remarks>This method attempts to load fixtures from a cached file. If the cache is empty or unavailable, it
	/// fetches the fixtures from an external source. The method supports various fixture types, such as completed,
	/// postponed, rearranged, and voided fixtures, and populates their respective details.</remarks>
	/// <param name="leagueId">The unique identifier of the league.</param>
	/// <param name="seasonId">The unique identifier of the season. If not provided, the current season of the league will be used.</param>
	/// <returns>A list of <see cref="Fixture"/> objects representing the fixtures for the specified league and season. If no
	/// fixtures are found, an empty list is returned.</returns>
	public async Task<List<Fixture>> GetAllFixtures(TT365LeagueId leagueId, TT365SeasonId? seasonId = null)
	{
		seasonId ??= (await GetLeague(leagueId))?.CurrentSeason.GetSeasonId();
		if (!seasonId.HasValue) {
			throw new ArgumentNullException(nameof(seasonId));
		}

		string filename = $@"{leagueId}_{seasonId}_fixtures_all.json";

		List<Fixture> fixtures = await LoadAsync<List<Fixture>?>(leagueId, null, filename) ?? [];

		if (fixtures is not []) { return fixtures; }

		FixturesViewOptions fvo = new()
		{
			DivisionName = "All Divisions",
			ClubId = "",
			TeamId = "",
			VenueId = "",
			ViewModeType = FixturesViewType.Advanced,
			HideCompletedFixtures = false,
			MergeDivisions = true,
			ShowByWeekNo = true
		};

		string url = $"Fixtures/{seasonId}/{fvo.DivisionName}?c=False&vm={fvo.ViewModeType}&d={fvo.DivisionName}&vn={fvo.VenueId}&cl={fvo.ClubId}&t={fvo.TeamId}&swn={fvo.ShowByWeekNo}&hc={fvo.HideCompletedFixtures}&md={fvo.MergeDivisions}";
		HtmlDocument? doc = await LoadAsync<HtmlDocument>(leagueId, url);

		if (string.IsNullOrWhiteSpace(doc?.Text)) { return fixtures; }

		if (doc.DocumentNode.SelectNodes("//div[@id='Fixtures']") is null) { return fixtures; }

		foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@id='Fixtures']") ?? EMPTY_NODE_COLLECTION) {
			foreach (HtmlNode fixtureNode in node.SelectNodes(".//div[contains(@class, 'fixture')]") ?? EMPTY_NODE_COLLECTION)
			{
				string nodeClass = fixtureNode.Attributes["class"].Value;

				if (nodeClass.HasClass("fixture")) {
					FixtureType fixtureType = nodeClass.HasClass("complete")
						? fixtureNode.SelectSingleNode("div[@class='spacer']/div[contains(@class,'voided')]") is not null
							? FixtureType.Void
							: FixtureType.Completed
						: fixtureNode.SelectSingleNode("div[@class='spacer']/div[contains(@class,'postponed')]") is not null
							? FixtureType.Postponed
							: fixtureNode.SelectSingleNode("div[@class='spacer']/div[contains(@class,'rearranged')]") is not null
								? FixtureType.Rearranged
								: FixtureType.Fixture;
					string fixtureDescription = fixtureNode.Descendants("meta").Where(x => x.Attributes["itemprop"].Value == "description").Single().Attributes["content"].Value;
					_ = DateOnly.TryParse(fixtureNode.Descendants("time").SingleOrDefault()?.Attributes["datetime"].Value, out DateOnly fixtureDate);
					string fixtureDivision = fixtureNode.SelectSingleNode("div[@class='div']")?.InnerText ?? "";
					string fixtureVenue = HttpUtility.HtmlDecode(fixtureNode.SelectSingleNode("div[@class='venue']/span/a")?.InnerText ?? "");

					HtmlNode? homeNode = fixtureNode.SelectSingleNode("div[@class='home']");
					string fixtureHomeTeam = HttpUtility.HtmlDecode(homeNode?.Descendants("div").Where(x => x.HasClass("teamName")).SingleOrDefault()?.InnerText) ?? "";

					HtmlNode? awayNode = fixtureNode.SelectSingleNode("div[@class='away']");
					string fixtureAwayTeam = HttpUtility.HtmlDecode(awayNode?.Descendants("div").Where(x => x.HasClass("teamName")).SingleOrDefault()?.InnerText) ?? "";

					Fixture fixture = new(fixtureDivision, fixtureDescription, fixtureDate, fixtureHomeTeam, fixtureAwayTeam, fixtureVenue);

					if (fixtureType is FixtureType.Fixture or FixtureType.Unknown) {
						fixtures.Add(fixture);
					}

					if (fixtureType is FixtureType.Completed) {
						CompletedFixture cf = fixture.ToCompleted();
						cf.ForHome = int.Parse(homeNode?.Descendants("div").Where(x => x.Attributes["class"].Value.Trim() == "score").SingleOrDefault()?.InnerText ?? "");
						cf.ForAway = int.Parse(awayNode?.Descendants("div").Where(x => x.Attributes["class"].Value.Trim() == "score").SingleOrDefault()?.InnerText ?? "");
						cf.CardURL = $"{TT365_COM}{fixtureNode.SelectSingleNode("div/div[@class='matchCardIcon']/a")?.Attributes["href"].Value.Trim() ?? ""}";
						HtmlNodeCollection? playerNodes = fixtureNode.SelectNodes(".//div[@itemprop='performer' and starts-with(@class, 'player')]");
						if (playerNodes is not null) {
							foreach (HtmlNode playerNode in playerNodes) {
								string playerName = playerNode.SelectSingleNode("span/a")?.InnerText ?? playerNode.SelectSingleNode("span")?.InnerText ?? "";
								playerName = FixPlayerName(playerName);
								string? playerIdString = playerNode.SelectSingleNode("span/a")?.GetAttributeValue("href", null!);
								int playerId = 0;
								_ = int.TryParse(playerNode.LastChild.InnerText.Replace("(", "").Replace(")", ""), out int setsWon);
								if (playerIdString is not null) {
									playerId = string.IsNullOrWhiteSpace(playerIdString) ? 0 : int.Parse(playerIdString.Split('/').LastOrDefault() ?? "");
								}

								bool playerPoM = playerNode.HasClass("pom");
                                MatchPlayer matchPlayer = new(playerName, playerId, setsWon, playerPoM);
								if (playerNode.ParentNode.HasClass("homeTeam")) {
									cf.HomePlayers.Add(matchPlayer);
								} else {
									cf.AwayPlayers.Add(matchPlayer);
								}
							}
						}

						fixtures.Add(cf);
					}

					if (fixtureType is FixtureType.Postponed) {
						PostponedFixture pf = fixture.ToPostponed();
						pf.Reason = HttpUtility.HtmlDecode(fixtureNode.SelectSingleNode("div[@class='spacer']/div[contains(@class,'postponed')]")?.Attributes["title"].Value.Trim()) ?? "";
						fixtures.Add(pf);
					}

					if (fixtureType is FixtureType.Rearranged) {
						RearrangedFixture rf = fixture.ToRearranged();
						string title = HttpUtility.HtmlDecode(fixtureNode.SelectSingleNode("div[@class='spacer']/div[contains(@class,'rearranged')]")?.Attributes["title"].Value.Trim()) ?? "";
						string[] tokens = title.Split([':', '-'], StringSplitOptions.TrimEntries);
						rf.Reason = tokens[^1];
						rf.OriginalDate = DateOnly.Parse(tokens[2].Split([' '])[0]);
						fixtures.Add(rf);
					}

					if (fixtureType is FixtureType.Void) {
						VoidFixture vf = fixture.ToVoid();
						vf.Reason = fixtureNode.SelectSingleNode("div[@class='spacer']/div[contains(@class,'voided')]")?.Attributes["title"].Value.Trim() ?? "";
						fixtures.Add(vf);
					}
				}
			}
		}

		string jsonString = JsonSerializer.Serialize(fixtures);
		_ = SaveFileToCache(jsonString, filename);

		return fixtures;
	}
}

enum FixtureType
{
	Unknown,
	Fixture,
	Completed,
	Postponed,
	Rearranged,
	Void
}
