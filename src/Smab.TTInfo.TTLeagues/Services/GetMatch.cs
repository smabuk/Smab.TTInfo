namespace Smab.TTInfo.TTLeagues.Services;

/// <summary>
/// Provides functionality to read match data from TTLeagues.
/// </summary>
public sealed partial class TTLeaguesReader
{
	/// <summary>
	/// Retrieves a match by TTInfo ID and match ID.
	/// </summary>
	/// <param name="ttinfoId">The TTInfo identifier for the league.</param>
	/// <param name="matchId">The match identifier.</param>
	/// <returns>The match if found; otherwise, <c>null</c>.</returns>
	internal async Task<MatchCard?> GetMatch(int matchId, string ttinfoId)
	{
		string fileName = $"{ttinfoId}_match_{matchId}.json";

		MatchCard? matchCard = await LoadJsonAsync<MatchCard>(
			ttinfoId,
			null,
			fileName);

		if (matchCard is null) {
			EnsureDefaultRequestHeaders(ttinfoId);
			Match? match = await httpClient.GetFromJsonAsync<Match>($"matches/{matchId}");
			if (match is null || match.Home.Score is null) {
				return null;
			}

			MatchResults? matchResults = await httpClient.GetFromJsonAsync<MatchResults>($"matches/{matchId}/results");
			List<MatchSet>? matchSets = await httpClient.GetFromJsonAsync<List<MatchSet>>($"matches/{matchId}/sets");

			if (match is not null && matchSets is not null && matchResults is not null) {
				matchCard = new(
					Id: match.Id,
					Match: match,
					Results: matchResults,
					Sets: matchSets
					);
				_ = SaveFileToCache(JsonSerializer.Serialize(matchCard), fileName);
			}
		}

		return matchCard;
	}
}
