namespace Smab.TTInfo.TTLeagues.Services;

/// <summary>
/// Provides methods to read data from TTLeagues.
/// </summary>
public sealed partial class TTLeaguesReader
{
	/// <summary>
	/// Retrieves player statistics for a given league and player.
	/// </summary>
	/// <param name="playerId">The player identifier.</param>
	/// <param name="ttinfoId">The TTInfo identifier for the league.</param>
	/// <param name="competitionId">The competition identifier.</param>
	/// <returns>Player statistics if found; otherwise, <c>null</c>.</returns>
	internal async Task<PlayerStats?> GetPlayerStats(string playerId, string ttinfoId, int competitionId)
	{
		string fileName = $"{ttinfoId}_{competitionId}_player_{playerId}_stats.json"; 

		return await LoadJsonAsync<PlayerStats>(
			ttinfoId,
			$"competitions/{competitionId}/player/{playerId}/stats",
			fileName);
	}
}
