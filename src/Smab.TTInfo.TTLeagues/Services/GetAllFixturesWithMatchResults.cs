namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<Fixtures?> GetAllFixturesWithMatchResults(string leagueId, int? competitionId = null)
	{
		Fixtures? fixtures = await GetAllFixtures(leagueId, competitionId);
		if (fixtures is null) {
			return null;
		}

		Fixtures? results = await GetAllResults(leagueId, competitionId);
		if (results is not null) {
			List<Match> matches = fixtures.Matches.ToList();
			Dictionary<int, Match> resultsMatches = results.Matches.ToDictionary(m => m.Id);
			for (int i = 0; i < matches.Count; i++) {
				if (resultsMatches.TryGetValue(matches[i].Id, out Match? m)) {
					matches[i] = m;
				}
			}
			return new(fixtures.Groups, matches, fixtures.Type, fixtures.Total);
		}

		return fixtures;
	}
}
