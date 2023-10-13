namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<League?> GetLeague(string ttinfoId)
	{
		string fileName = $"{ttinfoId}_league.json";

		League? league = await LoadJsonAsync<League>(
			ttinfoId,
			null,
			fileName);

		if (league is null)
		{
			using HttpClient client = CreateHttpClient(ttinfoId);
			TenantsHost?       tenantsHost         = await client.GetFromJsonAsync<TenantsHost>("tenants/host");
			WebsitesHost?      websitesHost        = await client.GetFromJsonAsync<WebsitesHost>("websites/host");
			List<Competition>? currentCompetitions = await client.GetFromJsonAsync<List<Competition>>("competitions/all");
			List<Competition>? archives            = await client.GetFromJsonAsync<List<Competition>>("competitions/archives");

			league = new(
				TTInfoId:             ttinfoId,
				TenantsHost:          tenantsHost,
				WebsitesHost:         websitesHost,
				CurrentCompetitions:  [.. currentCompetitions],
				ArchivedCompetitions: [.. archives]);
			_ = SaveFileToCache(JsonSerializer.Serialize(league), fileName);
		}

		return league;
	}
}
