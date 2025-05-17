namespace Smab.TTInfo.TTLeagues.Services;

/// <summary>
/// Provides methods to read TTLeagues data.
/// </summary>
public sealed partial class TTLeaguesReader
{
	/// <summary>
	/// Retrieves division standings for a given league and division.
	/// </summary>
	/// <param name="ttinfoId">The TTInfo identifier for the league.</param>
	/// <param name="divisionId">The division identifier.</param>
	/// <returns>Standings for the specified division.</returns>
	internal async Task<List<TeamStanding>?> GetDivisionStandings(int divisionId, string ttinfoId)
	{
		string fileName = $"{ttinfoId}_division_{divisionId}_standings.json";

		return await LoadJsonAsync<List<TeamStanding>?>(
			ttinfoId,
			$"divisions/{divisionId}/standings",
			fileName);
	}
}
