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
		seasonId ??= (await GetLeague(leagueId))?.GetCurrentSeasonId();
		if (seasonId is null) { return null; };

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
		HtmlNode? captainNode = teamNode.SelectNodes("//div[text()='Captain']")?.FirstOrDefault();
		team.Captain = captainNode?.NextSibling.NextSibling.InnerText ?? "";
		team.CaptainPhone = captainNode?.NextSibling?.NextSibling?.NextSibling?.NextSibling?.InnerText ?? "";
		team.CaptainEmailAddress = ExtractEmailAddress(teamNode.GetFirstNodeByClass("email"));

		HtmlNode? playertableNode = teamNode.Descendants("table").Where(t => t.SelectSingleNode("caption")?.InnerText.Contains("Players") ?? false).SingleOrDefault();
		if (playertableNode is not null) {
			foreach (HtmlNode playerRow in playertableNode.SelectSingleNode("tbody")?.SelectNodes("tr") ?? EMPTY_NODE_COLLECTION) {
				HtmlNode[] cells = [.. playerRow.Descendants("td")];
				bool hasPoM = cells.Length > 4;
				Player player = new()
				{
					Name = FixPlayerName(cells[0].InnerText.Trim()),
					PlayerURL = $"{TT365_COM}{cells[0].Descendants("a").SingleOrDefault()?.Attributes["href"].Value}",
					Played = int.Parse(cells[1].InnerText),
					WinPercentage = float.Parse(cells[2].InnerText.Replace("%", "")),
					LeagueRanking = 0, // no longer being tracked apparently (appears to be broken)
					PoMAwards = hasPoM ? cells[3].InnerText : "",
				};
				List<string>? form = [..
					from f in cells[hasPoM ? 4 : 3].Descendants("a")
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

				team.Players = [.. team.Players, player];
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
				CompletedFixture completedFixture = new CompletedFixture(team.DivisionName, "", resultDate, "", "", "") with {
					ForHome          = int.Parse(score.Split("-")[0]),
					ForAway          = int.Parse(score.Split("-")[1]),
					Other            = other,
					CardURL          = $"{TT365_COM}/{cells[hasPoM ? 6 : 5].Descendants("a").Single().Attributes["href"].Value}",
					PlayerOfTheMatch = hasPoM ? FixPlayerName(cells[5].InnerText) : "",
				};

				TeamResult teamResult = new(
					completedFixture,
					Opposition: cells[0].InnerText,
					HomeOrAway: cells[1].InnerText.ToLowerInvariant(),
					Points: int.Parse(cells[4].InnerText),
					IsVoid: isVoid);

				team.Results = [.. team.Results, teamResult];
			}
		}

		string jsonString = JsonSerializer.Serialize(team);
		_ = SaveFileToCache(jsonString, filename);

		return team;
	}

	/// <summary>
	/// Extracts an email address from a script tag within the specified HTML node.
	/// </summary>
	/// <remarks>This method searches for a script tag in the provided HTML node that dynamically generates a mailto
	/// link. It decodes the email address embedded in the script content and returns it as a plain string. If the HTML
	/// node does not contain a valid script tag or the email address cannot be extracted, an empty string is
	/// returned.</remarks>
	/// <param name="htmlNode">The HTML node to search for the script tag. Can be null.</param>
	/// <returns>The decoded email address extracted from the script tag, or an empty string if no valid email address is found.</returns>
	private static string ExtractEmailAddress(HtmlNode? htmlNode)
	{
		const string JOIN_TEXT = "' + '";
		const string SEARCH_TERM = "lto:";
		const char SINGLE_QUOTE = '\'';

		// format is in the form of a script tag that writes a mailto link, e.g.:
		// <script type="text/javascript">document.write('<a href="mai' + 'lto:' + '&#115;&#105;&#109;&#111;&#110;&#46;&#98;&#114;&#111;&#111;&#107;&#101;&#115;' + '&#64;' + '&#98;&#116;&#105;&#110;&#116;&#101;&#114;&#110;&#101;&#116;&#46;&#99;&#111;&#109;' +'">&#115;&#105;&#109;&#111;&#110;&#46;&#98;&#114;&#111;&#111;&#107;&#101;&#115;&#64;' + '&#98;&#116;&#105;&#110;&#116;&#101;&#114;&#110;&#101;&#116;&#46;&#99;&#111;&#109;</a>');</script>

		if (htmlNode is null) { return ""; }

		HtmlNode? scriptNode = htmlNode.Descendants("script").FirstOrDefault();
		if (scriptNode is null) { return ""; }

		string scriptContent = scriptNode.InnerText.Replace(JOIN_TEXT, "");
		int mailtoIndex = scriptContent.IndexOf(SEARCH_TERM, StringComparison.OrdinalIgnoreCase);
		if (mailtoIndex == -1) { return ""; }

		int startIndex = mailtoIndex + SEARCH_TERM.Length;
		int endIndex = scriptContent.IndexOf(SINGLE_QUOTE, startIndex);

		return endIndex switch
		{
			-1 => "",
			 _ => HttpUtility.HtmlDecode(scriptContent[startIndex..endIndex])
		};
	}
}
