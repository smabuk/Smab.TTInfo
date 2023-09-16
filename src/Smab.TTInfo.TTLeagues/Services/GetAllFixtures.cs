namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	public async Task<Fixtures> GetAllFixtures(string leagueId, long? competitionId = null)
	{
		Fixtures? fixtures;
		leagueId = leagueId.ToLowerInvariant();

		fixtures = await CreateHttpClient(leagueId)
			.GetFromJsonAsync<Fixtures>($"matches/?competitionId={competitionId}");

		//string? jsonString = LoadFile($"_test_fixtures_maidenhead.json");
		//if (jsonString is not null) {
		//	fixtures = JsonSerializer.Deserialize<Fixtures>(jsonString, jsonSerializerOptions);
		//}

		return fixtures ?? new();
	}
}
