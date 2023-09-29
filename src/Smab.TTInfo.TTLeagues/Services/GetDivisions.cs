namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<List<Division>> GetDivisions(string leagueId, long competitionId)
	{
		List<Division>? divisions;
		string fileName = $"{leagueId}_{competitionId}_divisions";

		divisions = await LoadJsonAsync<List<Division>>(
			leagueId,
			$"competitions/{competitionId}/divisions",
			fileName);

		if (divisions is not null && !divisions.First().TeamStandings.Any()) {
			using HttpClient client = CreateHttpClient(leagueId);
			foreach (Division division in divisions) {
				List<TeamStanding>? teamStandings = await client.GetFromJsonAsync<List<TeamStanding>>($"divisions/{division.Id}/standings");
				division.TeamStandings.AddRange(teamStandings!);
			}
		}

		string? jsonString = JsonSerializer.Serialize(divisions);
		_ = SaveFile(jsonString, fileName);
		return divisions ?? new();
	}
}

