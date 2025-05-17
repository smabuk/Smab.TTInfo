using Smab.TTInfo.TTLeagues.Models.TTLeagues;

namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	/// <summary>
	/// Retrieves a competition by TTInfo ID and competition ID.
	/// </summary>
	/// <param name="ttinfoId">The TTInfo identifier for the league.</param>
	/// <param name="competitionId">The unique identifier for the competition.</param>
	/// <returns>The <see cref="Competition"/> if found; otherwise, <c>null</c>.</returns>
	internal async Task<Competition?> GetCompetition(string ttinfoId, int competitionId)
	{
		string fileName = $"{ttinfoId}_{competitionId}_competition.json";

		return await LoadJsonAsync<Competition>(
			ttinfoId,
			$"competitions/{competitionId}",
			fileName);
	}
}
