using HtmlAgilityPack;

namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
	public async Task<League?> GetLeague(string ttinfoId)
	{
		string url;
		HtmlDocument? doc;
		string? jsonString;
		string fileName = $"league_{ttinfoId}.json";

		League? league = await LoadAsync<League>(
			ttinfoId,
			null,
			fileName);

		if (league is null || string.IsNullOrWhiteSpace(league.CurrentSeason.Id))
		{
			url = $"";
			doc = await LoadAsync<HtmlDocument>(
				ttinfoId,
				url,
				$@"{ttinfoId}.html");

			if (string.IsNullOrWhiteSpace(doc?.Text)) { return null; }

			string leagueURL = url;
			string leagueName =        HttpUtility.HtmlDecode(doc.DocumentNode.SelectSingleNode(@"//title").InnerText);
			string leagueDescription = HttpUtility.HtmlDecode(doc.DocumentNode.SelectSingleNode(@"//meta[@property='og:description']").GetAttributeValue("content", ""));
			string leagueTheme       = doc.DocumentNode.SelectSingleNode(@"//body").GetAttributeValue("class", "");
			string currentSeasonId   = doc.DocumentNode.SelectSingleNode(@$"//a[starts-with(@href,'{$"/{ttinfoId}/Tables/"}')]").GetAttributeValue("href", "");
			currentSeasonId = currentSeasonId[(currentSeasonId.LastIndexOf('/') + 1)..];

			league = new(ttinfoId, leagueName, leagueDescription, leagueURL, leagueTheme);

			string currentSeasonName = doc.DocumentNode.SelectSingleNode(@$"//a[starts-with(@href,'{$"/{ttinfoId}/Tables/"}')]").GetAttributeValue("title", "").Replace(" Tables", "");
			league.CurrentSeason = new(currentSeasonId, currentSeasonName)
			{
				Lookups = await GetLookupTables(ttinfoId, currentSeasonId)
			};

			HtmlDocument archives = await LoadAsync<HtmlDocument>(
				ttinfoId,
				$"Results/Archive",
				$@"{ttinfoId}_Archive.html")
				?? new();

			foreach (HtmlNode? item in archives.DocumentNode.SelectNodes(@"//td//a"))
			{
				string seasonId = item.GetAttributeValue("href", "");
				seasonId = seasonId[(seasonId.LastIndexOf('/') + 1)..];
				string seasonName = item.InnerText;
				league.Seasons.Add(new(seasonId, seasonName));
			}
		} else {
			league.CurrentSeason.Lookups = await GetLookupTables(ttinfoId, league.CurrentSeason.Id);
		}

		league.CurrentSeason.Divisions = await GetDivisions(ttinfoId, league.CurrentSeason.Id);

		jsonString = JsonSerializer.Serialize(league);
		_ = SaveFileToCache(jsonString, $"league_{ttinfoId}.json");
		_ = SaveFileToCache(jsonString, $"league_{ttinfoId}_{league.CurrentSeason.Id}.json");
		
		return league;
	}
}
