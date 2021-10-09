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
		}

		return league;
	}
}

