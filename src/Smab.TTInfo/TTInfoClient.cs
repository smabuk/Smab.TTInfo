using System.Net.Http.Json;

namespace Smab.TTInfo;

public sealed class TTInfoClient
{
	public HttpClient Client { get; }

	public TTInfoClient(HttpClient httpClient)
	{
		Client = httpClient;
	}

	public async Task<FixturesView?> GetFixtures(string TeamName = "")
	{
		return await Client.GetFromJsonAsync<FixturesView?>($"api/tt/fixtures/{TeamName}");
	}

	public async Task<Team?> GetTeam(string TeamName)
	{
		return await Client.GetFromJsonAsync<Team?>($"api/tt/team/{TeamName}");
	}

	public async Task<List<string>> GetTeamPlayers(string TeamName)
	{
		return await Client.GetFromJsonAsync<List<string>?>($"api/tt/teamplayerslist/{TeamName}") ?? new();
	}

}
