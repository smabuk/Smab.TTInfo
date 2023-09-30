namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<TeamStats?> GetTeamStats(int teamId, string leagueId, int competitionId)
	{
		string fileName = $"{leagueId}_{competitionId}_team_{teamId}_stats.json"; 
		TeamStats? teamStats = await LoadJsonAsync<TeamStats>(
			leagueId,
			$"competitions/{competitionId}/team/{teamId}/stats",
			fileName);

		return teamStats;
	}
}
