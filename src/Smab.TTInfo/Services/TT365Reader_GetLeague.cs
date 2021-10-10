using HtmlAgilityPack;

using Smab.TTInfo.Models.TT365;

namespace Smab.TTInfo;

public partial class TT365Reader
{
	public async Task<League?> GetLeague(string LeagueId)
	{

		string url = $"{"https"}://www.tabletennis365.com/{LeagueId}";
		HtmlDocument doc = await LoadPage(
			url,
			$@"{LeagueId}.html");

		if (string.IsNullOrWhiteSpace(doc.Text))
		{
			return null;
		}

		League league = new(LeagueId);

		league.URL = url;
		league.Title = doc.DocumentNode.SelectSingleNode(@"//title").InnerText.Replace("&amp;", "&");
		league.Description = doc.DocumentNode.SelectSingleNode(@"//meta[@property='og:description']").GetAttributeValue("content", "").Replace("&amp;", "&");
		league.Name = league.Title;
		league.Theme = doc.DocumentNode.SelectSingleNode(@"//body").GetAttributeValue("class", "");
		string currentSeasonId = doc.DocumentNode.SelectSingleNode(@$"//a[starts-with(@href,'{$"/{LeagueId}/Tables/"}')]").GetAttributeValue("href", "");
		currentSeasonId = currentSeasonId.Substring(currentSeasonId.LastIndexOf("/") + 1);
		string currentSeasonName = doc.DocumentNode.SelectSingleNode(@$"//a[starts-with(@href,'{$"/{LeagueId}/Tables/"}')]").GetAttributeValue("title", "").Replace (" Tables", "");
		league.CurrentSeason = new(currentSeasonId, currentSeasonName);

		foreach (HtmlNode? item in doc.DocumentNode.SelectNodes(@"//ul[./li[text()='Archive']]//a"))
		{
			string seasonId = item.GetAttributeValue("href", "");
			seasonId = seasonId.Substring(seasonId.LastIndexOf("/") + 1);
			string seasonName = item.GetAttributeValue("title", "");
			league.Seasons.Add(new(seasonId, seasonName));
		}


		url = $"{"https"}://www.tabletennis365.com/{LeagueId}/Tables/{league.CurrentSeason.Id}/All_Divisions";

		doc = new();
		doc = await LoadPage(
			url,
			$@"{LeagueId}_Divisions_All_Divisions.html");

		if (!string.IsNullOrWhiteSpace(doc.Text))
		{
			foreach (HtmlNode? divTable in doc.DocumentNode.SelectNodes(@"//table"))
			{
				if (divTable.SelectSingleNode("caption") is null)
				{
					continue;
				}

				string divName = divTable.SelectSingleNode("caption")?.InnerText.Split(">").Last().Trim() ?? "";
				Division division = new(divName);
				league.CurrentSeason.Divisions.Add(division);

				foreach (HtmlNode? teamRow in divTable.SelectNodes(@"tbody//tr"))
				{
					Team team = new();
					team.Name = teamRow.ChildNodes[3].FirstChild.InnerText.Trim();
					team.ShortName = teamRow.ChildNodes[3].ChildNodes[1].InnerText.Trim().Replace("&#39;", "'").Replace("&amp;", "&");
					team.URL = $"{"https"}://www.tabletennis365.com{teamRow.ChildNodes[3].FirstChild.FirstChild.GetAttributeValue("href", "")}";

					if (int.TryParse(teamRow.SelectSingleNode(@"td[contains(@class, 'pos')]")?.InnerText ?? "0", out int leaguePosition))
					{
						team.LeaguePosition = leaguePosition;
					};
					team.Played = int.Parse(teamRow.SelectSingleNode(@"td[contains(@class, 'played')]")?.InnerText ?? "0");
					team.Won = int.Parse(teamRow.SelectSingleNode(@"td[contains(@class, 'won')]")?.InnerText ?? "0");
					team.Drawn = int.Parse(teamRow.SelectSingleNode(@"td[contains(@class, 'drawn')]")?.InnerText ?? "0");
					team.Lost = int.Parse(teamRow.SelectSingleNode(@"td[contains(@class, 'lost')]")?.InnerText ?? "0");
					team.SetsFor = int.Parse(teamRow.SelectSingleNode(@"td[contains(@class, 'setsFor')]")?.InnerText ?? "0");
					team.SetsAgainst = int.Parse(teamRow.SelectSingleNode(@"td[contains(@class, 'setsAgainst')]")?.InnerText ?? "0");
					team.PointsAdjustment = int.Parse(teamRow.SelectSingleNode(@"td[contains(@class, 'pointAdj')]")?.InnerText ?? "0");
					team.Points = int.Parse(teamRow.SelectSingleNode(@"td[contains(@class, 'points')]")?.InnerText ?? "0");

					division.Teams.Add(team);
				}

			}



		}

		return league;
	}
}

