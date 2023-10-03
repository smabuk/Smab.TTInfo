namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<List<TeamStanding>?> GetDivisionStandings(int divisionId, string leagueId)
	{
		string fileName = $"{leagueId}_division_{divisionId}_standings.json";

		return await LoadJsonAsync<List<TeamStanding>?>(
			leagueId,
			$"divisions/{divisionId}/standings",
			fileName);
	}
}
