namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<List<Division>> GetDivisions(string leagueId, long competitionId)
	{
		List<Division> divisions;

		HttpClient client = CreateHttpClient(leagueId);
		divisions = (await client.GetFromJsonAsync<List<Division>>($"competitions/{competitionId}/divisions")) ?? new();

		if (divisions is not null ) {
			foreach (Division division in divisions) {
				List<TeamStanding>? teamStandings = await client.GetFromJsonAsync<List<TeamStanding>>($"divisions/{division.Id}/standings");
				division.TeamStandings.AddRange(teamStandings!);
			}
		}
		//LookupTables lookupTables = await GetLookupTables(LeagueId, SeasonId);

		//if (lookupTables.DivisionLookup.Count == 0) {
		//	return divisions;
		//}


		return divisions ?? new();
	}
}

