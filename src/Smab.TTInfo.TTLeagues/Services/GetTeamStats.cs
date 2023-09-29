namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<Team?> GetTeamStats(string leagueId, int teamId)
	{
		string filename = $"{leagueId}_team_{teamId}"; 
		Team? team = await LoadJsonAsync<Team>(
			leagueId,
			$"teams/{teamId}",
			filename);

		if (team is not null) {
			//foreach (Division division in divisions) {
			//	List<TeamStanding>? teamStandings = await client.GetFromJsonAsync<List<TeamStanding>>($"divisions/{division.Id}/standings");
			//	division.TeamStandings.AddRange(teamStandings!);
			//}
		}

		return team;
	}
}
