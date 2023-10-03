namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<PlayerStats?> GetPlayerStats(string playerId, string leagueId, int competitionId)
	{
		string fileName = $"{leagueId}_{competitionId}_player_{playerId}_stats.json"; 

		return await LoadJsonAsync<PlayerStats>(
			leagueId,
			$"competitions/{competitionId}/player/{playerId}/stats",
			fileName);
	}
}
