namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<Competition?> GetCompetition(string leagueId, int competitionId)
	{
		string fileName = $"{leagueId}_{competitionId}_competition.json";

		return await LoadJsonAsync<Competition>(
			leagueId,
			$"competitions/{competitionId}",
			fileName);
	}
}
