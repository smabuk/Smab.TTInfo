namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<Team?> GetTeam(int teamId, string leagueId)
	{
		string fileName = $"{leagueId}_team_{teamId}.json";

		return await LoadJsonAsync<Team>(
			leagueId,
			$"teams/{teamId}",
			fileName);
	}
}
