namespace Smab.TTInfo.TTLeagues.Services;

/// <summary>
/// Provides methods to read TTLeagues data.
/// </summary>
public sealed partial class TTLeaguesReader
{
	/// <summary>
	/// Retrieves team statistics for a given league and team.
	/// </summary>
	/// <param name="teamId">The team identifier.</param>
	/// <param name="ttinfoId">The TTInfo identifier for the league.</param>
	/// <param name="competitionId">The competition identifier.</param>
	/// <returns>Team statistics if found; otherwise, <c>null</c>.</returns>
	internal async Task<TeamStats?> GetTeamStats(int teamId, string ttinfoId, int competitionId)
	{
		string fileName = $"{ttinfoId}_{competitionId}_team_{teamId}_stats.json"; 

		return  await LoadJsonAsync<TeamStats>(
			ttinfoId,
			$"competitions/{competitionId}/team/{teamId}/stats",
			fileName);
	}
}
