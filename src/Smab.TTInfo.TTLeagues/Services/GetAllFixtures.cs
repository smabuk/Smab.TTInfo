namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<Fixtures?> GetAllFixtures(string leagueId, int? competitionId = null)
	{
		string fileName = competitionId is null
			? $"{leagueId}_fixtures.json"
			: $"{leagueId}_{competitionId}_fixtures.json";

		Fixtures? fixtures = await LoadJsonAsync<Fixtures>(
			leagueId,
			$"matches/?competitionId={competitionId}&type=1",
			fileName);

		Fixtures? results = await GetAllResults(leagueId, competitionId);

		if (fixtures is not null && results is not null) {
			Dictionary<int, Match> resultsMatches = results.Matches.ToDictionary(m => m.Id);
			for (int i = 0; i < fixtures.Matches.Count; i++) {
				if (resultsMatches.TryGetValue(fixtures.Matches[i].Id, out Match? m)) {
					fixtures.Matches[i] = m;
				}
			}
		}

		return fixtures;
	}
}
