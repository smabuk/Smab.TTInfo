using HtmlAgilityPack;

namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
	/// <summary>
	/// Retrieves lookup tables containing divisions, clubs, teams, and venues for a specified season and table tennis
	/// information ID.
	/// </summary>
	/// <remarks>This method attempts to load lookup data from the cache. If the data is not found, it fetches the
	/// data from the source, processes it, and stores it in the cache for subsequent calls. The returned lookup tables can
	/// be used to populate dropdowns or other UI elements for filtering fixtures.</remarks>
	/// <param name="leagueId">The unique identifier for the table tennis information source.</param>
	/// <param name="seasonId">The unique identifier for the season.</param>
	/// <returns>A <see cref="LookupTables"/> object containing lookup data for divisions, clubs, teams, and venues. If the data is
	/// not available in the cache, it is fetched from the source and cached for future use.</returns>
	public async Task<LookupTables> GetLookupTables(TT365LeagueId leagueId, TT365SeasonId seasonId)
	{
		LookupTables lookup = new();

		string? jsonString;
		jsonString = LoadFileFromCache($"{leagueId}_{seasonId}_lookup_divisions.json");
		if (jsonString is null) {
			FixturesViewOptions fvo = FixturesViewOptions.Create
			(
				season: seasonId,
				divisionName: "All Divisions",
				clubId: "",
				teamId: "",
				venueId: "",
				viewModeType: FixturesViewType.Advanced,
				hideCompletedFixtures: false,
				mergeDivisions: true,
				showByWeekNo: true
			);

			string url = $"Fixtures/{seasonId}/{fvo.DivisionName}?c=False&vm={fvo.ViewModeType}&d={fvo.DivisionName}&vn={fvo.VenueId}&cl={fvo.ClubId}&t={fvo.TeamId}&swn={fvo.ShowByWeekNo}&hc={fvo.HideCompletedFixtures}&md={fvo.MergeDivisions}";
			HtmlDocument? doc = await LoadAsync<HtmlDocument>(
				leagueId,
				url,
				$"{leagueId}_{seasonId}_fixtures_all.html"
			);

			if (!string.IsNullOrWhiteSpace(doc?.Text)) {
				HtmlNode? node = doc.DocumentNode.SelectSingleNode("//form[@id='FixtureFiltersForm']");
				if (node is not null) {
					foreach (HtmlNode item in node.SelectNodes("//select[@id='d']//option") ?? EMPTY_NODE_COLLECTION) {
						lookup.DivisionLookup = [.. lookup.DivisionLookup, new(item.GetAttributeValue("value", ""), item.InnerText)];
					}

					foreach (HtmlNode item in node.SelectNodes("//select[@id='cl']//option") ?? EMPTY_NODE_COLLECTION) {
						lookup.ClubLookup = [.. lookup.ClubLookup, new(item.GetAttributeValue("value", ""), item.InnerText)];
					}

					foreach (HtmlNode item in node.SelectNodes("//select[@id='t']//option") ?? EMPTY_NODE_COLLECTION) {
						lookup.TeamLookup = [.. lookup.TeamLookup, new(item.GetAttributeValue("value", ""), item.InnerText)];
					}

					foreach (HtmlNode item in node.SelectNodes("//select[@id='vn']//option") ?? EMPTY_NODE_COLLECTION) {
						lookup.VenueLookup = [.. lookup.VenueLookup, new(item.GetAttributeValue("value", ""), item.InnerText)];
					}

					_ = SaveFileToCache(JsonSerializer.Serialize(lookup.DivisionLookup), $"{leagueId}_{seasonId}_lookup_divisions.json");
					_ = SaveFileToCache(JsonSerializer.Serialize(lookup.VenueLookup),    $"{leagueId}_{seasonId}_lookup_venues.json");
					_ = SaveFileToCache(JsonSerializer.Serialize(lookup.ClubLookup),     $"{leagueId}_{seasonId}_lookup_clubs.json");
					_ = SaveFileToCache(JsonSerializer.Serialize(lookup.TeamLookup),     $"{leagueId}_{seasonId}_lookup_teams.json");
				}
			}
		} else {
			lookup.DivisionLookup = JsonSerializer.Deserialize<ImmutableList<IdNamePair>>(jsonString) ?? [];
			jsonString = LoadFileFromCache($"{leagueId}_{seasonId}_lookup_venues.json");
			if (jsonString is not null) {
				lookup.VenueLookup = JsonSerializer.Deserialize<ImmutableList<IdNamePair>>(jsonString) ?? [];
			}

			jsonString = LoadFileFromCache($"{leagueId}_{seasonId}_lookup_clubs.json");
			if (jsonString is not null) {
				lookup.ClubLookup = JsonSerializer.Deserialize<ImmutableList<IdNamePair>>(jsonString) ?? [];
			}

			jsonString = LoadFileFromCache($"{leagueId}_{seasonId}_lookup_teams.json");
			if (jsonString is not null) {
				lookup.TeamLookup = JsonSerializer.Deserialize<ImmutableList<IdNamePair>>(jsonString) ?? [];
			}
		}

		return lookup;
	}
}
