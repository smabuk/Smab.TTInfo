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

		url = $"{"https"}://www.tabletennis365.com/{LeagueId}/Tables/{league.CurrentSeason.Id}/All_Divisions";
		doc = await LoadPage(
		url,
			$@"{LeagueId}_{league.CurrentSeason.Id}_Divisions_All_Divisions.html");

		if (!string.IsNullOrWhiteSpace(doc.Text))
		{
			league.CurrentSeason.Divisions = new List<Division>();
			foreach (HtmlNode? divTable in doc.DocumentNode.SelectNodes(@"//table"))
			{
				if (divTable.SelectSingleNode("caption") is null)
				{
					continue;
				}

				string divName = divTable.SelectSingleNode("caption")?.InnerText.Split(">").Last().Trim() ?? "";
				Division division = new(Id: league.CurrentSeason.Lookups.DivisionLookup.Where(d => d.Name == divName).Single().Id, Name: divName);
				league.CurrentSeason.Divisions.Add(division);

				foreach (HtmlNode? teamRow in divTable.SelectNodes(@"tbody//tr"))
				{
					Team team = new();
					team.Name = teamRow.ChildNodes[3].FirstChild.InnerText.Trim();
					team.Id = league.CurrentSeason.Lookups.TeamLookup.Where(t => t.Name == team.Name).Single().Id;
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

					team.DivisionName = divName;

					division.Teams.Add(team);
				}

			}

		}

		jsonString = JsonSerializer.Serialize(league);
		_ = SaveFile(jsonString, $"league_{LeagueId}.json");
		jsonString = JsonSerializer.Serialize(league);
		_ = SaveFile(jsonString, $"league_{LeagueId}_{league.CurrentSeason.Id}.json");
		
		return league;
	}
}

