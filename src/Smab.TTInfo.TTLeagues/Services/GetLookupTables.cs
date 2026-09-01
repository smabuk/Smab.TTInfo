namespace Smab.TTInfo.TTLeagues.Services;

/// <summary>
/// Provides methods to read data from TTLeagues.
/// </summary>
public sealed partial class TTLeaguesReader
{
	/// <summary>
	/// Gets the lookup tables for a given TTInfo ID, including competitions, divisions, and teams. Caching: The archived
	/// lookup tables are cached for 1 month to improve performance and reduce API calls.
	/// </summary>
	/// <param name="ttinfoId">The TTInfo identifier for the league.</param>
	/// <returns>Lookup tables for the league.</returns>
	internal async Task<List<LookupValue>> GetLookupTables(string ttinfoId)
	{
		const int CACHE_HOURS_1_MONTH = 24 * 30; // 1 month cache duration
		string fileNameWithoutCompetitionId = $"{ttinfoId}_lookup_tables.json";

		List<LookupValue>? lookup = await LoadJsonAsync<List<LookupValue>>(
			ttinfoId,
			null,
			fileNameWithoutCompetitionId);

		if (lookup is null) {
			lookup = [];
			League? league = await GetLeague(ttinfoId);
			List<Competition> competitions = league?.CurrentCompetitions.Union(league.ArchivedCompetitions).ToList() ?? [];
			foreach (Competition competition in competitions) {
				string fileNameWithId = $"{ttinfoId}_{competition.Id}_lookup_tables.json";
				List<LookupValue> lookupById = await LoadJsonAsync<List<LookupValue>>(
						ttinfoId,
						null,
						fileNameWithId,
						cacheHours: CACHE_HOURS_1_MONTH)
					?? [];

				if (league!.CurrentCompetitions.Select(c => c.Id).Contains(competition.Id) || lookupById is []) {
					lookupById = [];
					lookupById.Add(new(LookupType.Competition, competition.Id, competition.Name));
					List<Division> divisions = await GetDivisions(ttinfoId, competition.Id);
					foreach (Division division in divisions) {
						lookupById.Add(new(LookupType.Division, division.Id, division.Name));
						List<TeamStanding>? teamStandings = await GetDivisionStandings(division.Id, ttinfoId);
						foreach (TeamStanding team in teamStandings ?? []) {
							lookupById.Add(new(LookupType.Team, team.TeamId, team.Name));
						}
					}

					_ = SaveFileToCache(JsonSerializer.Serialize(lookupById), fileNameWithId);
				}

				lookup.AddRange(lookupById);
			}

			_ = SaveFileToCache(JsonSerializer.Serialize(lookup), fileNameWithoutCompetitionId);
		}

		return lookup;
	}
}
