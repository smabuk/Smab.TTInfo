namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<List<LookupValue>> GetLookupTables(string ttinfoId)
	{
		string fileNameWithoutCompetitionId = $"{ttinfoId}_lookup_tables.json";

		List<LookupValue>? lookup = await LoadJsonAsync<List<LookupValue>>(
			ttinfoId,
			null,
			fileNameWithoutCompetitionId);

		if (lookup is null) {
			lookup = [];
			League? league = await GetLeague(ttinfoId);
			foreach (Competition competition in league?.CurrentCompetitions ?? []) {
				lookup.Add(new(LookupType.Competition, competition.Id, competition.Name));
				List<Division> divisions = await GetDivisions(ttinfoId, competition.Id);
				foreach (Division division in divisions) {
					lookup.Add(new(LookupType.Division, division.Id, division.Name));
					List<TeamStanding>? teamStandings = await GetDivisionStandings(division.Id, ttinfoId);
					foreach (TeamStanding team in teamStandings ?? []) {
						lookup.Add(new(LookupType.Team, team.TeamId, team.Name));
					}
				}

				_ = SaveFileToCache(JsonSerializer.Serialize(lookup), $"{ttinfoId}_{competition.Id}_lookup_tables.json");
			}

			_ = SaveFileToCache(JsonSerializer.Serialize(lookup), fileNameWithoutCompetitionId);
		}

		return lookup;
	}
}
