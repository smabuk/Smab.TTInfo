namespace Smab.TTInfo.TTLeagues.Services;

/// <summary>
/// Provides methods to read TTLeagues data.
/// </summary>
public sealed partial class TTLeaguesReader
{
	/// <summary>
	/// Retrieves all fixtures with match results for a given league.
	/// </summary>
	/// <param name="ttinfoId">The TTInfo identifier for the league.</param>
	/// <param name="competitionId">The optional competition identifier.</param>
	/// <returns>A collection of fixtures with match results.</returns>
	public async Task<Fixtures?> GetAllFixturesWithMatchResults(string ttinfoId, int? competitionId = null)
	{
		Fixtures? fixtures = await GetAllFixtures(ttinfoId, competitionId);
		if (fixtures is null) {
			return null;
		}

		Fixtures? results = await GetAllResults(ttinfoId, competitionId);
		if (results is not null) {
			List<Match> matches = [.. fixtures.Matches];
			Dictionary<int, Match> resultsMatches = results.Matches.ToDictionary(m => m.Id);
			for (int i = 0; i < matches.Count; i++) {
				if (resultsMatches.TryGetValue(matches[i].Id, out Match? m)) {
					matches[i] = m;
				}
			}

			fixtures = fixtures with { Matches = [.. matches] };
		}

		return fixtures;
	}
}
