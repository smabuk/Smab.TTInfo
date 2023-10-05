namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<PlayerStats?> GetPlayerStats(string playerId, string ttinfoId, int competitionId)
	{
		string fileName = $"{ttinfoId}_{competitionId}_player_{playerId}_stats.json"; 

		return await LoadJsonAsync<PlayerStats>(
			ttinfoId,
			$"competitions/{competitionId}/player/{playerId}/stats",
			fileName);
	}
}
