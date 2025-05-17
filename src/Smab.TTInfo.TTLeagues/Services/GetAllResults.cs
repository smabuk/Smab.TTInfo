namespace Smab.TTInfo.TTLeagues.Services;

/// <summary>
/// Provides methods to read data from TTLeagues.
/// </summary>
public sealed partial class TTLeaguesReader
{
	/// <summary>
	/// Retrieves all results for a given league.
	/// </summary>
	/// <param name="ttinfoId">The TTInfo identifier for the league.</param>
	/// <param name="competitionId">The optional competition identifier.</param>
	/// <returns>A collection of results.</returns>
	internal async Task<Fixtures?> GetAllResults(string ttinfoId, int? competitionId = null)
	{
		string fileName = competitionId is null
			? $"{ttinfoId}_results.json"
			: $"{ttinfoId}_{competitionId}_results.json";

		return await LoadJsonAsync<Fixtures>(
			ttinfoId,
			$"matches/?competitionId={competitionId}&type=2",
			fileName);
	}
}
