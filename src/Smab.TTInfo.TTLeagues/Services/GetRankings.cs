namespace Smab.TTInfo.TTLeagues.Services;

/// <summary>
/// Provides methods to read TTLeagues data.
/// </summary>
public sealed partial class TTLeaguesReader
{
	/// <summary>
	/// Retrieves rankings for a given league.
	/// </summary>
	/// <param name="ttinfoId">The TTInfo identifier for the league.</param>
	/// <returns>A collection of rankings.</returns>
	internal async Task<List<Ranking>> GetRankings(string ttinfoId)
	{
		string fileName = $"{ttinfoId}_rankings.json";

		return await LoadJsonAsync<List<Ranking>>(
			ttinfoId,
			$"rankings",
			fileName) ?? [];
	}
}
