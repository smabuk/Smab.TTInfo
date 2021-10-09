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
			currentSeasonId = item.GetAttributeValue("href", "");
			currentSeasonId = currentSeasonId.Substring(currentSeasonId.LastIndexOf("/") + 1);
			currentSeasonName = item.GetAttributeValue("title", "");
			league.Seasons.Add(new(currentSeasonId, currentSeasonName));
		}


		url = $"{"https"}://www.tabletennis365.com/{LeagueId}/Tables/{league.CurrentSeason}/All_Divisions";

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
					team.URL = $"{"https"}://www.tabletennis365.com{teamRow.ChildNodes[3].FirstChild.FirstChild.GetAttributeValue("href", "")}";

					if (int.TryParse(teamRow.ChildNodes[1].InnerText, out int leaguePosition))
					{
						team.LeaguePosition = leaguePosition;
					};
					team.Played = int.Parse(teamRow.ChildNodes[5].InnerText);
					team.Won = int.Parse(teamRow.ChildNodes[7].InnerText);
					team.Drawn = int.Parse(teamRow.ChildNodes[9].InnerText);
					team.Lost = int.Parse(teamRow.ChildNodes[11].InnerText);
					team.SetsFor = int.Parse(teamRow.ChildNodes[13].InnerText);
					team.SetsAgainst = int.Parse(teamRow.ChildNodes[15].InnerText);
					team.Points = int.Parse(teamRow.ChildNodes[17].InnerText);

					division.Teams.Add(team);
				}

			}



		}

		return league;
	}
}

