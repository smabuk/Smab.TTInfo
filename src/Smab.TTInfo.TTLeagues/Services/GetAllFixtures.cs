using Smab.TTInfo.TTLeagues.Models;

namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<Fixtures> GetAllFixtures(string leagueId, long? competitionId = null)
	{
		Fixtures? fixtures;
		string cacheFilename = competitionId is null
			? $"fixtures_{leagueId}.json"
			: $"fixtures_{leagueId}_{competitionId}.json";
		leagueId = leagueId.ToLowerInvariant();
		string? jsonString = LoadFile(cacheFilename);
		if (jsonString is not null) {
			fixtures = JsonSerializer.Deserialize<Fixtures>(jsonString, jsonSerializerOptions);
		} else { 
			HttpClient client = CreateHttpClient(leagueId);
			jsonString = await client.GetStringAsync($"matches/?competitionId={competitionId}");
			fixtures = JsonSerializer.Deserialize<Fixtures>(jsonString);
			_ = SaveFile(jsonString, cacheFilename);
		}

		return fixtures ?? new();
	}
}
