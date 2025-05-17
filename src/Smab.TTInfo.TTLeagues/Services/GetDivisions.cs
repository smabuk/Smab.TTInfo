namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	/// <summary>
	/// Retrieves division information for a given league.
	/// </summary>
	/// <param name="ttinfoId">The TTInfo identifier for the league.</param>
	/// <param name="competitionId">The competition identifier.</param>
	/// <returns>A collection of divisions.</returns>
	internal async Task<List<Division>> GetDivisions(string ttinfoId, int competitionId)
	{
		string fileName = $"{ttinfoId}_{competitionId}_divisions.json";

		return await LoadJsonAsync<List<Division>>(
			ttinfoId,
			$"competitions/{competitionId}/divisions",
			fileName) ?? [];
	}
}

