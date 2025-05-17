namespace Smab.TTInfo.TTLeagues.Services;

/// <summary>
/// Provides methods to read TTLeagues data.
/// </summary>
public sealed partial class TTLeaguesReader
{
	/// <summary>
	/// Retrieves a team by TTInfo ID and team ID.
	/// </summary>
	/// <param name="teamId">The team identifier.</param>
	/// <param name="ttinfoId">The TTInfo identifier for the league.</param>
	/// <returns>The team if found; otherwise, <c>null</c>.</returns>
	internal async Task<Team?> GetTeam(int teamId, string ttinfoId)
	{
		string fileName = $"{ttinfoId}_team_{teamId}.json";

		return await LoadJsonAsync<Team>(
			ttinfoId,
			$"teams/{teamId}",
			fileName);
	}
}
