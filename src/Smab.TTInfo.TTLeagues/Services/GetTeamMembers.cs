namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<List<TeamMember>> GetTeamMembers(int teamId, string ttinfoId)
	{
		string fileName = $"{ttinfoId}_team_{teamId}_members.json";

		return await LoadJsonAsync<List<TeamMember>>(
			ttinfoId,
			$"teams/{teamId}/members",
			fileName) ?? [];
	}
}
