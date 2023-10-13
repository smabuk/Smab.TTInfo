namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<MatchCard?> GetMatch(int matchId, string leagueId)
	{
		string fileName = $"{leagueId}_match_{matchId}.json";

		MatchCard? matchCard = await LoadJsonAsync<MatchCard>(
			leagueId,
			null,
			fileName);

		if (matchCard is null)
		{
			using HttpClient client = CreateHttpClient(leagueId);

			Match? match = await client.GetFromJsonAsync<Match>($"matches/{matchId}");
			if (match is null || match.Home.Score is null) {
				return null;
			}
			MatchResults? matchResults = await client.GetFromJsonAsync<MatchResults>($"matches/{matchId}/results");
			List<MatchSet>?  matchSets = await client.GetFromJsonAsync<List<MatchSet>>($"matches/{matchId}/sets");

			if (match is not null && matchSets is not null && matchResults is not null) {
				matchCard = new(
					Id:      match.Id,
					Match:   match,
					Results: matchResults,
					Sets:    matchSets
					);
				_ = SaveFileToCache(JsonSerializer.Serialize(matchCard), fileName);
			}
		}

		return matchCard;
	}
}
