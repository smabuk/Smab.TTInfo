using HtmlAgilityPack;

namespace Smab.TTInfo;

public sealed partial class TT365Reader
{
	public async Task<Team?> GetTeamStats(string LeagueId, string TeamName, string SeasonId = "")
	{
		Team team = new();

		if (string.IsNullOrWhiteSpace(SeasonId)) {
			League? league = await GetLeague(LeagueId);
			if (league is null) { return null; };
			SeasonId = league.CurrentSeason.Id;
		}

		List<Division> divisions = await GetDivisions(LeagueId, SeasonId);
		if (divisions.Count == 0) {
			return null;
		};

		HttpClient client = new();
		string lookupTeamName = TeamName.Replace("_", " ");

		team = divisions.SelectMany(d => d.Teams).SingleOrDefault(t => t.Name.ToUpperInvariant() == TeamName.ToUpperInvariant()) ?? new();

		HtmlDocument doc = await LoadPage(
			team.URL,
			$@"{LeagueId}_{SeasonId}_TeamStats_{TeamName}.html");

		HtmlNode? teamNode = doc.DocumentNode.SelectSingleNode("//div[@id='TeamStats']");

		if (teamNode == null) {
			return team;
		}

		// fixture.Description = fixtureNode.SelectSingleNode("//meta[@itemprop='description']").Attributes("content").Value
		foreach (HtmlNode? node in doc.DocumentNode.SelectNodes("//div[@id='TeamStats']")) {
			team.Caption = node.SelectSingleNode("//div[@class='caption']").InnerText.Replace("&gt;", ">");
			team.Players = new List<Player>();
			team.Results = new List<TeamResult>();
			try {
				team.Captain = teamNode.SelectNodes("//div[text()='Captain']").Single().NextSibling.NextSibling.InnerText;
			}
			catch (Exception) {
			}
			// For Each playerRow In node.SelectNodes("//tbody/tr")
			HtmlNode? playertableNode = node.Descendants("table").Where(t => t.SelectSingleNode("caption").InnerText.Contains("Players")).SingleOrDefault();
			if (playertableNode is not null) {
				foreach (HtmlNode playerRow in playertableNode.SelectSingleNode("tbody").SelectNodes("tr")) {
					HtmlNode[] cells = playerRow.Descendants("td").ToArray();
					bool hasPoM = cells.Length > 5;
					Player player = new()
					{
						Name = FixPlayerName(cells[0].InnerText.Trim()),
						PlayerURL = $"{"https"}://www.tabletennis365.com{cells[0].Descendants("a").SingleOrDefault()?.Attributes["href"].Value}",
						Played = int.Parse(cells[1].InnerText),
						WinPercentage = float.Parse(cells[2].InnerText.Replace("%", "")),
						LeagueRanking = int.Parse(cells[3].InnerText),
						PoMAwards = hasPoM ? cells[4].InnerText : "",
					};
					List<string>? form = (from f in cells[hasPoM ? 5 : 4].Descendants("a")
										  select f.InnerText).ToList();
					player.Form = string.Join(",", form);
					List<string> rankings = (from r in cells[3].Descendants("a")
											 select r.Attributes["data-content"].Value).FirstOrDefault()?.Replace("<br />", "|").Split("|").ToList() ?? new();
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

			HtmlNode? resultstableNode = node.Descendants("table").SingleOrDefault(t => t.SelectSingleNode("caption").InnerText.Contains("Results"));
			if (resultstableNode is not null) {
				foreach (HtmlNode? resultRow in resultstableNode.SelectSingleNode("tbody").SelectNodes("tr")) {
					HtmlNode[] cells = resultRow.Descendants("td").ToArray();
					string score = cells[3].InnerText;
					string? other = null;
					if (score.ToLowerInvariant() == "void") {
						score = "0 - 0";
						other = cells[3].Attributes["title"]?.Value;
					}
					if (score.EndsWith(" (A)")) {
						score = score.Replace(" (A)", "");
						other = cells[3].Attributes["title"]?.Value;
					}
					bool hasPoM = cells.Length > 6;
					TeamResult result = new()
					{
						Opposition = cells[0].InnerText,
						HomeOrAway = cells[1].InnerText,
						ForHome    = int.Parse(score.Split("-")[0]),
						ForAway    = int.Parse(score.Split("-")[1]),
						Points     = int.Parse(cells[4].InnerText),
						PlayerOfTheMatch = hasPoM ? cells[5].InnerText : "",
						Other = other,
						CardURL = $"{tt365com}/{cells[hasPoM ? 6 : 5].Descendants("a").Single().Attributes["href"].Value}"
					};
					if (DateOnly.TryParse(cells[2].InnerText,
						   gbCulture,
						   System.Globalization.DateTimeStyles.None,
						   out DateOnly tempDate)) {
						result.Date = tempDate;
					};
					team.Results.Add(result);
				}
			}
		}

		return team;
	}
}
