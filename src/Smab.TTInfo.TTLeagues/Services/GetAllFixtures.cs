namespace Smab.TTInfo.TTLeagues.Services;

/// <summary>
/// Provides methods to read data from TTLeagues.
/// </summary>
public sealed partial class TTLeaguesReader
{
	/// <summary>
	/// Retrieves all fixtures for a given league.
	/// </summary>
	/// <param name="ttinfoId">The TTInfo identifier for the league.</param>
	/// <param name="competitionId">The optional competition identifier.</param>
	/// <returns>A collection of fixtures.</returns>
	internal async Task<Fixtures?> GetAllFixtures(string ttinfoId, int? competitionId = null)
	{
		string fileName = competitionId is null
			? $"{ttinfoId}_fixtures.json"
			: $"{ttinfoId}_{competitionId}_fixtures.json";

		return await LoadJsonAsync<Fixtures>(
			ttinfoId,
			$"matches/?competitionId={competitionId}&type=1",
			fileName);
	}
}
