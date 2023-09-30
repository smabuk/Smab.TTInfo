namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<List<Division>> GetDivisions(string leagueId, int competitionId)
	{
		string fileName = $"{leagueId}_{competitionId}_divisions.json";

		List<Division>? divisions = await LoadJsonAsync<List<Division>>(
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

		_ = SaveFile(JsonSerializer.Serialize(divisions), fileName);
		return divisions ?? new();
	}
}

