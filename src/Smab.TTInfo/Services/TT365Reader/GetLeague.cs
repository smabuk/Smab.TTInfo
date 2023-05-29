using HtmlAgilityPack;

namespace Smab.TTInfo;

public sealed partial class TT365Reader
{
	public async Task<League?> GetLeague(string LeagueId)
	{
		string url;
		HtmlDocument doc;
		League league;
		League? cachedLeague = null;

		string? jsonString = LoadFile($"league_{LeagueId}.json");
		if (jsonString is not null )
		{
			cachedLeague = JsonSerializer.Deserialize<League>(jsonString);
		}

		if (cachedLeague is null || string.IsNullOrWhiteSpace(cachedLeague.CurrentSeason.Id))
		{
			url = $"{"https"}://www.tabletennis365.com/{LeagueId}";
			doc = await LoadPage(
				url,
				$@"{LeagueId}.html",
				240);

			if (string.IsNullOrWhiteSpace(doc.Text)) { return null; }

			string leagueURL = url;
			string leagueName = doc.DocumentNode.SelectSingleNode(@"//title").InnerText.Replace("&amp;", "&");
			string leagueDescription = doc.DocumentNode.SelectSingleNode(@"//meta[@property='og:description']").GetAttributeValue("content", "").Replace("&amp;", "&");
			string leagueTheme = doc.DocumentNode.SelectSingleNode(@"//body").GetAttributeValue("class", "");
			string currentSeasonId = doc.DocumentNode.SelectSingleNode(@$"//a[starts-with(@href,'{$"/{LeagueId}/Tables/"}')]").GetAttributeValue("href", "");
			currentSeasonId = currentSeasonId[(currentSeasonId.LastIndexOf("/") + 1)..];

			league = new(LeagueId, leagueName, leagueDescription, leagueURL, leagueTheme);


			string currentSeasonName = doc.DocumentNode.SelectSingleNode(@$"//a[starts-with(@href,'{$"/{LeagueId}/Tables/"}')]").GetAttributeValue("title", "").Replace(" Tables", "");
			league.CurrentSeason = new(currentSeasonId, currentSeasonName)
			{
				Lookups = await GetLookupTables(LeagueId, currentSeasonId)
			};

			foreach (HtmlNode? item in doc.DocumentNode.SelectNodes(@"//ul[./li[text()='Archive']]//a"))
			{
				string seasonId = item.GetAttributeValue("href", "");
				seasonId = seasonId[(seasonId.LastIndexOf("/") + 1)..];
				string seasonName = item.GetAttributeValue("title", "");
				league.Seasons.Add(new(seasonId, seasonName));
			}

		} else {
			league = cachedLeague;
			league.CurrentSeason.Lookups = await GetLookupTables(LeagueId, league.CurrentSeason.Id);
		}


		league.CurrentSeason.Divisions = await GetDivisions(LeagueId, league.CurrentSeason.Id);

		jsonString = JsonSerializer.Serialize(league);
		_ = SaveFile(jsonString, $"league_{LeagueId}.json");
		jsonString = JsonSerializer.Serialize(league);
		_ = SaveFile(jsonString, $"league_{LeagueId}_{league.CurrentSeason.Id}.json");
		
		return league;
	}
}

