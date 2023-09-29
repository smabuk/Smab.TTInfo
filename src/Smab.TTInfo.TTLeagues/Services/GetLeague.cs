namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<League?> GetLeague(string LeagueId)
	{
		League league;
		League? cachedLeague = null;
		HttpClient client = CreateHttpClient(LeagueId);

		string? jsonString = LoadFile($"league_{LeagueId}.json");
		if (jsonString is not null )
		{
			cachedLeague = JsonSerializer.Deserialize<League>(jsonString);
		}

		if (cachedLeague is null || cachedLeague.CurrentCompetitionId == 0)
		{
			TenantsHost? tenantsHost = await client.GetFromJsonAsync<TenantsHost>("tenants/host");
			WebsitesHost? websitesHost = await client.GetFromJsonAsync<WebsitesHost>("websites/host");
			List<Competition>? currentCompetitions = await client.GetFromJsonAsync<List<Competition>>("competitions/all");
			List<Competition>? archives = await client.GetFromJsonAsync<List<Competition>>("competitions/archives");

			league = new()
			{
				Id = LeagueId,
				Competitions = [..currentCompetitions, ..archives],
				CurrentCompetition = currentCompetitions?.Single()!,
				CurrentCompetitionId = currentCompetitions?.Single().Id ?? 0,
				TenantsHost = tenantsHost,
				WebsitesHost = websitesHost,
			};
			jsonString = JsonSerializer.Serialize(league);
			_ = SaveFile(jsonString, $"league_{LeagueId}.json");
			_ = SaveFile(jsonString, $"league_{LeagueId}_{league.CurrentCompetitionId}.json");
		} else {
			league = cachedLeague;
		}

		List<Division>? divisions = await GetDivisions(LeagueId, league.CurrentCompetitionId);
		league.CurrentCompetition.Divisions = divisions;

		return league;
	}
}
