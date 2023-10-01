namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<Fixtures?> GetAllResults(string leagueId, int? competitionId = null)
	{
		string fileName = competitionId is null
			? $"{leagueId}_results.json"
			: $"{leagueId}_{competitionId}_results.json";

		return await LoadJsonAsync<Fixtures>(
			leagueId,
			$"matches/?competitionId={competitionId}&type=2",
			fileName);
	}
}
