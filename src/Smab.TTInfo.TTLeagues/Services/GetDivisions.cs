namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<List<Division>> GetDivisions(string leagueId, int competitionId)
	{
		string fileName = $"{leagueId}_{competitionId}_divisions.json";

		List<Division>? divisions = await LoadJsonAsync<List<Division>>(
			leagueId,
			$"competitions/{competitionId}/divisions",
			fileName);

		return divisions ?? [];
	}
}

