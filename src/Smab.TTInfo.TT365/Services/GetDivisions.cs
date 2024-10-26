using HtmlAgilityPack;

using Smab.TTInfo.TT365.Models.TT365;

namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
	public async Task<List<Division>> GetDivisions(string ttinfoId, string SeasonId = "")
	{
		LookupTables lookupTables = await GetLookupTables(ttinfoId, SeasonId);
		string filename = $@"{ttinfoId}_{SeasonId}_divisions_all.json";
		if (lookupTables.DivisionLookup.Count == 0) { return []; }

		List<Division> divisions = await LoadAsync<List<Division>?>(ttinfoId, null, filename) ?? [];
		if (divisions is not []) { return divisions; }

		string url = $"Tables/{SeasonId}/All_Divisions";
		HtmlDocument? doc = await LoadAsync<HtmlDocument>(
			ttinfoId,
			url);

		if (!string.IsNullOrWhiteSpace(doc?.Text)) {
			foreach (HtmlNode? divTable in doc.DocumentNode.SelectNodes(@"//table")) {
				if (divTable.SelectSingleNode("caption") is null) {
					continue;
				}

				string divName = divTable.SelectSingleNode("caption")?.InnerText.Split(">").Last().Trim() ?? "";
				Division division = new(Id: lookupTables.DivisionLookup.Where(d => d.Name == divName).Single().Id, Name: divName);
				divisions.Add(division);

				foreach (HtmlNode? teamRow in divTable.SelectNodes(@"tbody//tr")) {
					Team team = new()
					{
						DivisionName = divName,
						Name = teamRow.ChildNodes[3].FirstChild.InnerText.Trim(),
					};
					team.Id        = lookupTables.TeamLookup.Where(t => t.Name == team.Name).Single().Id;
					team.ShortName = HttpUtility.HtmlDecode(teamRow.ChildNodes[3].ChildNodes[1].InnerText.Trim());
					team.URL       = $"{TT365_COM}{teamRow.ChildNodes[3].FirstChild.FirstChild.GetAttributeValue("href", "")}";

					if (int.TryParse(teamRow.SelectSingleNode(@"td[contains(@class, 'pos')]")?.InnerText ?? "0", out int leaguePosition)) {
						team.LeaguePosition = leaguePosition;
					};
					team.Played           = int.Parse(teamRow.SelectSingleNode(@"td[contains(@class, 'played')]")?.InnerText ?? "0");
					team.Won              = int.Parse(teamRow.SelectSingleNode(@"td[contains(@class, 'won')]")?.InnerText ?? "0");
					team.Drawn            = int.Parse(teamRow.SelectSingleNode(@"td[contains(@class, 'drawn')]")?.InnerText ?? "0");
					team.Lost             = int.Parse(teamRow.SelectSingleNode(@"td[contains(@class, 'lost')]")?.InnerText ?? "0");
					team.SetsFor          = int.Parse(teamRow.SelectSingleNode(@"td[contains(@class, 'setsFor')]")?.InnerText ?? "0");
					team.SetsAgainst      = int.Parse(teamRow.SelectSingleNode(@"td[contains(@class, 'setsAgainst')]")?.InnerText ?? "0");
					team.PointsAdjustment = int.Parse(teamRow.SelectSingleNode(@"td[contains(@class, 'pointAdj')]")?.InnerText ?? "0");
					team.Points           = int.Parse(teamRow.SelectSingleNode(@"td[contains(@class, 'points')]")?.InnerText ?? "0");

					division.Teams.Add(team);
				}
			}
		}

		string jsonString = JsonSerializer.Serialize(divisions);
		_ = SaveFileToCache(jsonString, filename);

		return divisions;
	}
}

