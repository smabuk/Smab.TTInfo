namespace Smab.TTInfo;

public sealed partial class TTLeaguesReader
{
	public async Task<Fixtures> GetAllFixtures(string LeagueId, string? SeasonId = null)
	{
		Fixtures? fixtures = null;

		JsonSerializerOptions options = new()
		{
			ReadCommentHandling = JsonCommentHandling.Skip,
			PropertyNameCaseInsensitive = true,
		};

		string? jsonString = LoadFile($"_test_fixtures_maidenhead.json");
		if (jsonString is not null) {
			fixtures = JsonSerializer.Deserialize<Fixtures>(jsonString, options);
		}

		return fixtures ?? new();
	}
}
