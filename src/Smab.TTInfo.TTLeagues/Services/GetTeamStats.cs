namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<TeamStats?> GetTeamStats(int teamId, string ttinfoId, int competitionId)
	{
		string fileName = $"{ttinfoId}_{competitionId}_team_{teamId}_stats.json"; 

		return  await LoadJsonAsync<TeamStats>(
			ttinfoId,
			$"competitions/{competitionId}/team/{teamId}/stats",
			fileName);
	}
}
