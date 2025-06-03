using HtmlAgilityPack;

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
		if (lookupTables.DivisionLookup.Count == 0) { return []; }

		string filename = $@"{leagueId}_{seasonId}_divisions_all.json";
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
				string divId   = lookupTables.DivisionLookup.Where(d => d.Name == divName).Single().Id;

				List<Team> teams = [];
				foreach (HtmlNode? teamRow in divTable.SelectNodes(@"tbody//tr") ?? EMPTY_NODE_COLLECTION) {
					string teamName = teamRow.ChildNodes[3].FirstChild.InnerText.Trim();
					Team team = new()
					{
						DivisionName     = divName,
						Name             = teamName,
						Id               = lookupTables.TeamLookup.Where(t => t.Name == teamName).Single().Id,
						ShortName        = HttpUtility.HtmlDecode(teamRow.ChildNodes[3].ChildNodes[1].InnerText.Trim()),
						URL              = $"{TT365_COM}{teamRow.ChildNodes[3].FirstChild.FirstChild.GetAttributeValue("href", "")}",
						LeaguePosition   = teamRow.GetIntValueOrDefault("pos", null),
						Played           = teamRow.GetIntValue("played"),
						Won              = teamRow.GetIntValue("won"),
						Drawn            = teamRow.GetIntValue("drawn"),
						Lost             = teamRow.GetIntValue("lost"),
						SetsFor          = teamRow.GetIntValue("setsFor"),
						SetsAgainst      = teamRow.GetIntValue("setsAgainst"),
						PointsAdjustment = teamRow.GetIntValue("pointAdj"),
						Points           = teamRow.GetIntValue("points"),
					};

					teams.Add(team);
				}

				divisions.Add(new(divId, divName, [.. teams]));
			}
		}

		string jsonString = JsonSerializer.Serialize(divisions);
		_ = SaveFileToCache(jsonString, filename);

		return divisions;
	}
}
