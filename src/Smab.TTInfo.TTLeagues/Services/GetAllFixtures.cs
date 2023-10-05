namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<Fixtures?> GetAllFixtures(string leagueId, int? competitionId = null)
	{
		string fileName = competitionId is null
			? $"{leagueId}_fixtures.json"
			: $"{leagueId}_{competitionId}_fixtures.json";

		return await LoadJsonAsync<Fixtures>(
			leagueId,
			$"matches/?competitionId={competitionId}&type=1",
			fileName);
	}
}
