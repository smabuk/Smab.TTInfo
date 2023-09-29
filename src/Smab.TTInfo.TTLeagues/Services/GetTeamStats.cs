namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<Team?> GetTeamStats(string leagueId, int teamId)
	{
		Team? team;

		HttpClient client = CreateHttpClient(leagueId);
		team = await client.GetFromJsonAsync<Team>($"teams/{teamId}");

		if (team is not null) {
			//foreach (Division division in divisions) {
			//	List<TeamStanding>? teamStandings = await client.GetFromJsonAsync<List<TeamStanding>>($"divisions/{division.Id}/standings");
			//	division.TeamStandings.AddRange(teamStandings!);
			//}
		}

		return team;
	}
}
