using HtmlAgilityPack;

namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
	public async Task<LookupTables> GetLookupTables(string ttinfoId, string seasonId)
	{
		LookupTables lookup = new();

		string? jsonString;
		jsonString = LoadFileFromCache($"league_{ttinfoId}_{seasonId}_lookup_divisions.json");
		if (jsonString is null) {
			FixturesViewOptions fvo = new()
			{
				DivisionName = "All Divisions",
				ClubId = "",
				TeamId = "",
				VenueId = "",
				ViewModeType = FixturesViewType.Advanced,
				HideCompletedFixtures = false,
				MergeDivisions = true,
				ShowByWeekNo = true
			};

			string url = $"Fixtures/{seasonId}/{fvo.DivisionName}?c=False&vm={fvo.ViewModeType}&d={fvo.DivisionName}&vn={fvo.VenueId}&cl={fvo.ClubId}&t={fvo.TeamId}&swn={fvo.ShowByWeekNo}&hc={fvo.HideCompletedFixtures}&md={fvo.MergeDivisions}";
			HtmlDocument doc = await LoadAsync<HtmlDocument>(
				ttinfoId,
				url,
				$@"{ttinfoId}_{seasonId}_Fixtures_All_Divisions.html")
				?? new();

			if (!string.IsNullOrWhiteSpace(doc.Text)) {
				HtmlNode node = doc.DocumentNode.SelectSingleNode("//form[@id='FixtureFiltersForm']");
				if (node is not null) {
					foreach (var item in node.SelectNodes("//select[@id='d']//option")) {
						lookup.DivisionLookup.Add(new(item.GetAttributeValue("value", ""), item.InnerText));
					}
					foreach (var item in node.SelectNodes("//select[@id='cl']//option")) {
						lookup.ClubLookup.Add(new(item.GetAttributeValue("value", ""), item.InnerText));
					}
					foreach (var item in node.SelectNodes("//select[@id='t']//option")) {
						lookup.TeamLookup.Add(new(item.GetAttributeValue("value", ""), item.InnerText));
					}
					foreach (var item in node.SelectNodes("//select[@id='vn']//option")) {
						lookup.VenueLookup.Add(new(item.GetAttributeValue("value", ""), item.InnerText));
					}
					_ = SaveFileToCache(JsonSerializer.Serialize(lookup.DivisionLookup), $"league_{ttinfoId}_{seasonId}_lookup_divisions.json");
					_ = SaveFileToCache(JsonSerializer.Serialize(lookup.VenueLookup),    $"league_{ttinfoId}_{seasonId}_lookup_venues.json");
					_ = SaveFileToCache(JsonSerializer.Serialize(lookup.ClubLookup),     $"league_{ttinfoId}_{seasonId}_lookup_clubs.json");
					_ = SaveFileToCache(JsonSerializer.Serialize(lookup.TeamLookup),     $"league_{ttinfoId}_{seasonId}_lookup_teams.json");
				}
			}
		} else {
			lookup.DivisionLookup = JsonSerializer.Deserialize<List<IdNamePair>>(jsonString) ?? [];
			jsonString = LoadFileFromCache($"league_{ttinfoId}_{seasonId}_lookup_venues.json");
			if (jsonString is not null) {
				lookup.VenueLookup = JsonSerializer.Deserialize<List<IdNamePair>>(jsonString) ?? [];
			}
			jsonString = LoadFileFromCache($"league_{ttinfoId}_{seasonId}_lookup_clubs.json");
			if (jsonString is not null) {
				lookup.ClubLookup = JsonSerializer.Deserialize<List<IdNamePair>>(jsonString) ?? [];
			}
			jsonString = LoadFileFromCache($"league_{ttinfoId}_{seasonId}_lookup_teams.json");
			if (jsonString is not null) {
				lookup.TeamLookup = JsonSerializer.Deserialize<List<IdNamePair>>(jsonString) ?? [];
			}
		}

		return lookup;
	}
}
