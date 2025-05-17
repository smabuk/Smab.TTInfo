namespace Smab.TTInfo.TTLeagues.Services;

/// <summary>
/// Provides methods to read TTLeagues data.
/// </summary>
public sealed partial class TTLeaguesReader
{
	/// <summary>
	/// Retrieves team members for a given league and team.
	/// </summary>
	/// <param name="ttinfoId">The TTInfo identifier for the league.</param>
	/// <param name="teamId">The team identifier.</param>
	/// <returns>A collection of team members.</returns>
	internal async Task<List<TeamMember>> GetTeamMembers(int teamId, string ttinfoId)
	{
		string fileName = $"{ttinfoId}_team_{teamId}_members.json";

		return await LoadJsonAsync<List<TeamMember>>(
			ttinfoId,
			$"teams/{teamId}/members",
			fileName) ?? [];
	}
}
