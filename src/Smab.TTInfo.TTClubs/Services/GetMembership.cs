namespace Smab.TTInfo.TTClubs.Services;

/// <summary>
/// Provides functionality to read match data from TTClubs.
/// </summary>
public sealed partial class TTClubsReader
{
	public async Task<Membership?> GetMembership(string ttinfoId, int id)
	{
		string fileName = $"{ttinfoId}_membership_{id}.json";

		return await LoadJsonAsync<Membership>(
			ttinfoId,
			$"memberships/members?count&membershipId={id}",
			fileName,
			cacheHours: 0
			);
	}
}
