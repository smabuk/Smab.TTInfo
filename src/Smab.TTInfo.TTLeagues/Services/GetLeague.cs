namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<League?> GetLeague(string LeagueId)
	{
		League league;
		League? cachedLeague = null;
		string fileName = $"{LeagueId}_league.json";

		using HttpClient client = CreateHttpClient(LeagueId);

		string? jsonString = LoadFile(fileName);
		if (jsonString is not null )
		{
			cachedLeague = JsonSerializer.Deserialize<League>(jsonString);
		}

		if (cachedLeague is null)
		{
			TenantsHost? tenantsHost = await client.GetFromJsonAsync<TenantsHost>("tenants/host");
			WebsitesHost? websitesHost = await client.GetFromJsonAsync<WebsitesHost>("websites/host");
			List<Competition>? currentCompetitions = await client.GetFromJsonAsync<List<Competition>>("competitions/all");
			List<Competition>? archives = await client.GetFromJsonAsync<List<Competition>>("competitions/archives");

			league = new()
			{
				Id = LeagueId,
				TenantsHost = tenantsHost,
				WebsitesHost = websitesHost,
				CurrentCompetitions = [.. currentCompetitions],
				ArchivedCompetitions = [.. archives],
			};
			jsonString = JsonSerializer.Serialize(league);
			_ = SaveFile(jsonString, fileName);
		} else {
			league = cachedLeague;
		}

		return league;
	}
}
