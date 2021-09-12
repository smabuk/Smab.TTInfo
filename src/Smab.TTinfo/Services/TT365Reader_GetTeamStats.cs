using HtmlAgilityPack;

using Smab.TTInfo.Models.TT365;

namespace Smab.TTInfo;

public partial class TT365Reader
{
	public async Task<Team?> GetTeamStats(string TeamName)
	{
		Team team = new();

		string Division = "";
		string ActualName = "";

		HttpClient client = new();
		string lookupTeamName = TeamName.Replace("_", " ");
		Dictionary<string, TeamInfo> TeamInfoLookup;

		TeamInfoLookup = GetTeamInfoForSeason(Season);

		if (TeamInfoLookup.ContainsKey(lookupTeamName))
		{
			Division = TeamInfoLookup[lookupTeamName].Division;
			ActualName = TeamInfoLookup[lookupTeamName].Name;
		}
		else
			return null; /* TODO Change to default(_) if this is not a reference type */

		HtmlDocument doc = await LoadPage(
			$"{"https"}://www.tabletennis365.com/{League}/Results/Team/Statistics/{Season.Replace(" ", "_")}/{Division.Replace(" ", "_")}/{ActualName.Replace(" ", "_")}",
			$@"{League}_TeamStats_{TeamName}.html");

		HtmlNode? teamNode = doc.DocumentNode.SelectSingleNode("//div[@id='TeamStats']");

		if (teamNode == null)
			return team;

		// fixture.Description = fixtureNode.SelectSingleNode("//meta[@itemprop='description']").Attributes("content").Value
		foreach (HtmlNode? node in doc.DocumentNode.SelectNodes("//div[@id='TeamStats']"))
		{
			team.Caption = node.SelectSingleNode("//div[@class='caption']").InnerText.Replace("&gt;", ">");
			team.DivisionName = Division;
			team.Name = TeamName;
			team.Players = new List<Player>();
			team.Results = new List<Result>();
			try
			{
				team.Captain = teamNode.SelectNodes("//div[text()='Captain']").Single().NextSibling.NextSibling.InnerText;
			}
			catch (Exception)
			{
			}
			// For Each playerRow In node.SelectNodes("//tbody/tr")
			HtmlNode? playertableNode = node.Descendants("table").Where(t => t.SelectSingleNode("caption").InnerText.Contains("Players")).SingleOrDefault();
			if (playertableNode != null)
			{
				foreach (HtmlNode playerRow in playertableNode.SelectSingleNode("tbody").SelectNodes("tr"))
				{
					HtmlNode[] cells = playerRow.Descendants("td").ToArray();
					Player player = new()
					{
						Name = cells[0].InnerText,
						Played = int.Parse(cells[1].InnerText),
						WinPercentage = cells[2].InnerText,
						LeagueRanking = int.Parse(cells[3].InnerText),
						PoMAwards = cells[4].InnerText
					};
					player.Name = player.Name.Replace("&#39;", "'");
					player.Name = player.Name.Replace("Osullivan", "O'Sullivan");
					player.Name = player.Name.Replace("Ohalloran", "O'Halloran");
					List<string>? form = (from f in cells[5].Descendants("a")
										  select f.InnerText).ToList();
					player.Form = string.Join(",", form);
					List<string> rankings = (from r in cells[3].Descendants("a")
											 select r.Attributes["data-content"].Value).FirstOrDefault()?.Replace("<br />", "|").Split("|").ToList() ?? new();
					foreach (string? rank in rankings)
					{
						if (rank.Contains(':'))
						{
							string[]? rTemp = rank.Split(":");
							string rankType = rTemp[0].Trim();
							string rankValue = rTemp[1].Trim();
							switch (rankType)
							{
								case "OLOP":
									{
										player.ClubRanking = int.Parse(rankValue);
										break;
									}
								case "Reading":
									{
										player.LeagueRanking = int.Parse(rankValue);
										break;
									}
								case "Berkshire":
									{
										player.CountyRanking = int.Parse(rankValue);
										break;
									}
								case "TTE > South East Region":
									{
										player.RegionalRanking = int.Parse(rankValue);
										break;
									}
								case "National":
									{
										player.NationalRanking = int.Parse(rankValue);
										break;
									}
							}
						}
					}
					team.Players.Add(player);
				}
			}

			HtmlNode? resultstableNode = node.Descendants("table").Where(t => t.SelectSingleNode("caption").InnerText.Contains("Results")).SingleOrDefault();
			if (resultstableNode != null)
			{
				foreach (HtmlNode? resultRow in resultstableNode.SelectSingleNode("tbody").SelectNodes("tr"))
				{
					HtmlNode[] cells = resultRow.Descendants("td").ToArray();
					Result result = new()
					{
						Opposition = cells[0].InnerText,
						HomeOrAway = cells[1].InnerText,
						ScoreForHome = cells[3].InnerText.Split("-")[0].Trim(),
						ScoreForAway = cells[3].InnerText.Split("-")[1].Trim(),
						Points = int.Parse(cells[4].InnerText),
						PlayerOfTheMatch = cells[5].InnerText,
						CardURL = $"{"https"}://www.tabletennis365.com/{cells[6].Descendants("a").Single().Attributes["href"].Value}"
					};
					DateTime tempDate;
					if (DateTime.TryParse(cells[2].InnerText, out tempDate))
					{
						result.Date = tempDate;
					};
					team.Results.Add(result);
				}
			}
		}

		return team;
	}
}

