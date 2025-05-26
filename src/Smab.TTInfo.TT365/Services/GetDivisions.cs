using HtmlAgilityPack;

using Smab.TTInfo.TT365.Models.TT365;

namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
	/// <summary>
	/// Retrieves a list of divisions for a specified table tennis information ID and optional season ID.
	/// </summary>
	/// <remarks>This method attempts to load division data from a cached file. If the data is not available in the
	/// cache, it fetches the division information from an external source, processes it, and saves it to the cache for
	/// future use. The method ensures that the returned list of divisions is populated with the relevant teams and their
	/// associated details.</remarks>
	/// <param name="leagueId">The unique identifier for the table tennis information. This parameter is required.</param>
	/// <param name="seasonId">The identifier for the season. This parameter is required.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Division"/>
	/// objects representing the divisions associated with the specified table tennis information and season.</returns>
	public async Task<List<Division>> GetDivisions(TT365LeagueId leagueId, TT365SeasonId seasonId)
	{
		LookupTables lookupTables = await GetLookupTables(leagueId, seasonId);
		string filename = $@"{leagueId}_{seasonId}_divisions_all.json";
		if (lookupTables.DivisionLookup.Count == 0) { return []; }

		List<Division> divisions = await LoadAsync<List<Division>?>(leagueId, null, filename) ?? [];
		if (divisions is not []) { return divisions; }

		string url = $"Tables/{seasonId}/All_Divisions";
		HtmlDocument? doc = await LoadAsync<HtmlDocument>(
			leagueId,
			url);

		if (!string.IsNullOrWhiteSpace(doc?.Text)) {
			foreach (HtmlNode? divTable in doc.DocumentNode.SelectNodes(@"//table") ?? EMPTY_NODE_COLLECTION) {
				if (divTable.SelectSingleNode("caption") is null) {
					continue;
				}

				string divName = divTable.SelectSingleNode("caption")?.InnerText.Split(">").Last().Trim() ?? "";
				Division division = new(Id: lookupTables.DivisionLookup.Where(d => d.Name == divName).Single().Id, Name: divName);
				divisions.Add(division);

				foreach (HtmlNode? teamRow in divTable.SelectNodes(@"tbody//tr") ?? EMPTY_NODE_COLLECTION) {
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

