using HtmlAgilityPack;

namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
	/// <summary>
	/// Retrieves detailed statistics for a specified team in a given season.
	/// </summary>
	/// <remarks>This method fetches team statistics by first determining the appropriate season (if not explicitly
	/// provided), then retrieving the relevant divisions and team data. If the team is not found in the divisions, an
	/// empty <see cref="Team"/> object is returned. The method also attempts to load additional details such as players,
	/// results, and rankings from the associated HTML document.</remarks>
	/// <param name="leagueId">The unique identifier for the table tennis information source.</param>
	/// <param name="teamName">The name of the team for which statistics are to be retrieved. This parameter is case-insensitive.</param>
	/// <param name="seasonId">The unique identifier for the season. If not provided, the current season for the league associated with <paramref
	/// name="leagueId"/> will be used.</param>
	/// <returns>A <see cref="Team"/> object containing the team's statistics, including players, results, and rankings. Returns
	/// <see langword="null"/> if the team or season cannot be found.</returns>
	public async Task<Team?> GetTeamStats(TT365LeagueId leagueId, string teamName, TT365SeasonId? seasonId = null)
	{
		if (seasonId is null) {
			League? league = await GetLeague(leagueId);
			if (league is null) { return null; };
			seasonId = league.CurrentSeason.GetSeasonId();
		}

		List<Division> divisions = await GetDivisions(leagueId, seasonId ?? new());
		if (divisions.Count == 0) {
			return null;
		}

		string filename = $@"{leagueId}_{seasonId}_team_stats_{teamName}.json";
		Team team = await LoadAsync<Team>(leagueId, null, filename) ?? null!;

		if (team is not null) { return team; }

		team = new();
		string lookupTeamName = teamName.Replace("_", " ");

		team = divisions.SelectMany(d => d.Teams).SingleOrDefault(t => t.Name.Equals(teamName, StringComparison.InvariantCultureIgnoreCase)) ?? new();

		HtmlDocument doc = await LoadAsync<HtmlDocument>(
				leagueId,
				team.URL)
			?? new();

		HtmlNode? teamNode = doc.DocumentNode.GetFirstNodeById("TeamStats");

		if (teamNode is null) { return team; }

		team.Caption = teamNode.GetFirstNodeByClass("caption")?.InnerText.Replace("&gt;", ">") ?? "";
		team.Players = [];
		team.Results = [];
		team.Captain = teamNode.SelectNodes("//div[text()='Captain']")?.First()?.NextSibling.NextSibling.InnerText ?? "";

		HtmlNode? playertableNode = teamNode.Descendants("table").Where(t => t.SelectSingleNode("caption")?.InnerText.Contains("Players") ?? false).SingleOrDefault();
		if (playertableNode is not null) {
			foreach (HtmlNode playerRow in playertableNode.SelectSingleNode("tbody")?.SelectNodes("tr") ?? EMPTY_NODE_COLLECTION) {
				HtmlNode[] cells = [.. playerRow.Descendants("td")];
				bool hasPoM = cells.Length > 5;
				Player player = new()
				{
					Name = FixPlayerName(cells[0].InnerText.Trim()),
					PlayerURL = $"{TT365_COM}{cells[0].Descendants("a").SingleOrDefault()?.Attributes["href"].Value}",
					Played = int.Parse(cells[1].InnerText),
					WinPercentage = float.Parse(cells[2].InnerText.Replace("%", "")),
					LeagueRanking = int.Parse(cells[3].InnerText),
					PoMAwards = hasPoM ? cells[4].InnerText : "",
				};
				List<string>? form = [.. 
					from f in cells[hasPoM ? 5 : 4].Descendants("a")
					select f.InnerText];
				player.Form = string.Join(",", form);
				List<string> rankings = (from r in cells[3].Descendants("a")
									select r.Attributes["data-content"].Value).FirstOrDefault()?.Replace("<br />", "|").Split("|").ToList() ?? [];
				foreach (string? rank in rankings) {
					if (rank.Contains(':')) {
						string[]? rTemp = rank.Split(":");
						string rankType = rTemp[0].Trim();
						if (int.TryParse(rTemp[1].Trim(), out int rankValue)) {
							switch (rankType) {
								case "OLOP":
								case "OLOP TTC": {
										player.ClubRanking = rankValue;
										break;
									}
								case "Reading": {
										player.LeagueRanking = rankValue;
										break;
									}
								case "Berkshire": {
										player.CountyRanking = rankValue;
										break;
									}
								case "TTE > South East Region":
								case "South East": {
										player.RegionalRanking = rankValue;
										break;
									}
								case "National": {
										player.NationalRanking = rankValue;
										break;
									}
								default:
									break;
							}
						}
					}
				}

				team.Players.Add(player);
			}
		}

		HtmlNode? resultstableNode = teamNode.Descendants("table").SingleOrDefault(t => t.SelectSingleNode("caption")?.InnerText.Contains("Results") ?? false);
		if (resultstableNode is not null) {
			foreach (HtmlNode? resultRow in resultstableNode.SelectSingleNode("tbody")?.SelectNodes("tr") ?? EMPTY_NODE_COLLECTION) {
				HtmlNode[] cells = [.. resultRow.Descendants("td")];
				string score = cells[3].InnerText;
				string? other = null;
				bool isVoid = false;
				if (score.Equals("void", StringComparison.OrdinalIgnoreCase)) {
					score = "0 - 0";
					other = cells[3].Attributes["title"]?.Value;
					isVoid = true;
				}

				if (score.EndsWith(" (A)")) {
					score = score.Replace(" (A)", "");
					other = cells[3].Attributes["title"]?.Value;
				}

				_ = DateOnly.TryParse(cells[2].InnerText,
						GB_CULTURE,
						System.Globalization.DateTimeStyles.None,
						out DateOnly resultDate);

				bool hasPoM = cells.Length > 6;
				TeamResult teamResult = new(team.DivisionName, "", resultDate, "", "", "")
				{
					// Set the properties for the CompletedFixture base record
					ForHome          = int.Parse(score.Split("-")[0]),
					ForAway          = int.Parse(score.Split("-")[1]),
					Other            = other,
					CardURL          = $"{TT365_COM}/{cells[hasPoM ? 6 : 5].Descendants("a").Single().Attributes["href"].Value}",
					PlayerOfTheMatch = hasPoM ? FixPlayerName(cells[5].InnerText) : "",

					// Set the properties for the TeamResult record
					Points     = int.Parse(cells[4].InnerText),
					Opposition = cells[0].InnerText,
					HomeOrAway = cells[1].InnerText,
					IsVoid     = isVoid,
				};

				team.Results.Add(teamResult);
			}
		}

		string jsonString = JsonSerializer.Serialize(team);
		_ = SaveFileToCache(jsonString, filename);

		return team;
	}
}
