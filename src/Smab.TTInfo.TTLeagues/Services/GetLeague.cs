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
			EnsureDefaultRequestHeaders(ttinfoId);
			TenantsHost?       tenantsHost         = await LoadJsonAsync<TenantsHost>(ttinfoId, "tenants/host", $"{ttinfoId}_league_tenants.json");
			WebsitesHost?      websitesHost        = await httpClient.GetFromJsonAsync<WebsitesHost>("websites/host");
			List<Competition>? currentCompetitions = await httpClient.GetFromJsonAsync<List<Competition>>("competitions/all");
			List<Competition>? archives            = await httpClient.GetFromJsonAsync<List<Competition>>("competitions/archives");

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
