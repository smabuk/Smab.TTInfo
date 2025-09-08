namespace Smab.TTInfo.TTClubs.Services;

/// <summary>
/// Provides functionality to read match data from TTClubs.
/// </summary>
public sealed partial class TTClubsReader
{
	public async Task<Memberships?> GetMemberships(string ttinfoId)
	{
		string fileName = $"{ttinfoId}_memberships.json";

		return await LoadJsonAsync<Memberships>(
			ttinfoId,
			$"memberships?public=true",
			fileName);
	}
}
