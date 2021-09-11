using System.Net.Http.Json;

namespace Smab.TTInfo;

public class TT365Client
{
	public HttpClient Client { get; }

	public TT365Client(HttpClient httpClient)
	{
		Client = httpClient;
	}

	public async Task<TT365Models.ReadingTeam?> GetReadingTeam(string TeamName)
	{
		return await Client.GetFromJsonAsync<TT365Models.ReadingTeam?>($"api/tt/readingteam/{TeamName}");
	}

	public async Task<TT365Models.FixturesView?> GetFixtures(string TeamName = "")
	{
		return await Client.GetFromJsonAsync<TT365Models.FixturesView?>($"api/tt/fixtures/{TeamName}");
	}

	public async Task<TT365Models.Team?> GetTeam(string TeamName)
	{
		return await Client.GetFromJsonAsync<TT365Models.Team?>($"api/tt/team/{TeamName}");
	}

	public async Task<List<string>> GetTeamPlayers(string TeamName)
	{
		return await Client.GetFromJsonAsync<List<string>?>($"api/tt/teamplayerslist/{TeamName}") ?? new();
	}

}
