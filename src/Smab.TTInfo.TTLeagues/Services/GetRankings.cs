﻿namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<List<Ranking>> GetRankings(string leagueId)
	{
		string fileName = $"{leagueId}_rankings.json";
		List<Ranking>? rankings = await LoadJsonAsync<List<Ranking>>(
			leagueId,
			$"rankings",
			fileName);

		return rankings ?? [];
	}
}