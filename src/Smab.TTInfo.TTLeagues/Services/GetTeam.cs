namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<Team?> GetTeam(int teamId, string ttinfoId)
	{
		string fileName = $"{ttinfoId}_team_{teamId}.json";

		return await LoadJsonAsync<Team>(
			ttinfoId,
			$"teams/{teamId}",
			fileName);
	}
}
