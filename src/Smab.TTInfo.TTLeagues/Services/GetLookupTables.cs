namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<List<LookupValue>> GetLookupTables(string leagueId)
	{
		string fileNameWithoutCompetitionId = $"{leagueId}_lookup_tables.json";

		List<LookupValue>? lookup = await LoadJsonAsync<List<LookupValue>>(
			leagueId,
			null,
			fileNameWithoutCompetitionId);

		if (lookup is null) {
			lookup = [];
			League? league = await GetLeague(leagueId);
			foreach (Competition competition in league?.CurrentCompetitions ?? []) {
				lookup.Add(new(LookupType.Competition, competition.Id, competition.Name));
				List<Division> divisions = await GetDivisions(leagueId, competition.Id);
				foreach (Division division in divisions) {
					lookup.Add(new(LookupType.Division, division.Id, division.Name));
					List<TeamStanding>? teamStandings = await GetDivisionStandings(division.Id, leagueId);
					foreach (TeamStanding team in teamStandings ?? []) {
						lookup.Add(new(LookupType.Team, team.TeamId, team.Name));
					}
				}
				_ = SaveFile(JsonSerializer.Serialize(lookup), $"{leagueId}_{competition.Id}_lookup_tables.json");
			}
			_ = SaveFile(JsonSerializer.Serialize(lookup), fileNameWithoutCompetitionId);
		}

		return lookup;
	}
}
