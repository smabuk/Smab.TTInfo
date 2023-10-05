namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<League?> GetLeague(string leagueId)
	{
		string fileName = $"{leagueId}_league.json";

		League? league = await LoadJsonAsync<League>(
			leagueId,
			null,
			fileName);

		if (league is null)
		{
			using HttpClient client = CreateHttpClient(leagueId);
			TenantsHost?       tenantsHost         = await client.GetFromJsonAsync<TenantsHost>("tenants/host");
			WebsitesHost?      websitesHost        = await client.GetFromJsonAsync<WebsitesHost>("websites/host");
			List<Competition>? currentCompetitions = await client.GetFromJsonAsync<List<Competition>>("competitions/all");
			List<Competition>? archives            = await client.GetFromJsonAsync<List<Competition>>("competitions/archives");

			league = new(
				TTInfoId:                   leagueId,
				TenantsHost:          tenantsHost,
				WebsitesHost:         websitesHost,
				CurrentCompetitions:  [.. currentCompetitions],
				ArchivedCompetitions: [.. archives]);
			_ = SaveFile(JsonSerializer.Serialize(league), fileName);
		}

		return league;
	}
}
