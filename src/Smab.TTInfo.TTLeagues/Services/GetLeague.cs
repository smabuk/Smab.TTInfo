namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<League?> GetLeague(string leagueId)
	{
		League league;
		string fileName = $"{leagueId}_league.json";

		League? cachedLeague = await LoadJsonAsync<League>(
			leagueId,
			null,
			fileName);

		if (cachedLeague is null)
		{
			using HttpClient client = CreateHttpClient(leagueId);
			TenantsHost? tenantsHost = await client.GetFromJsonAsync<TenantsHost>("tenants/host");
			WebsitesHost? websitesHost = await client.GetFromJsonAsync<WebsitesHost>("websites/host");
			List<Competition>? currentCompetitions = await client.GetFromJsonAsync<List<Competition>>("competitions/all");
			List<Competition>? archives = await client.GetFromJsonAsync<List<Competition>>("competitions/archives");

			league = new(
				Id: leagueId,
				TenantsHost: tenantsHost,
				WebsitesHost: websitesHost,
				CurrentCompetitions: [.. currentCompetitions],
				ArchivedCompetitions: [.. archives]);
			_ = SaveFile(JsonSerializer.Serialize(league), fileName);
		} else {
			league = cachedLeague;
		}

		return league;
	}
}
