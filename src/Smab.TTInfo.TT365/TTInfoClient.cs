using System.Net.Http.Json;

namespace Smab.TTInfo.TT365;

public sealed class TTInfoClient(HttpClient httpClient)
{
	public HttpClient Client { get; } = httpClient;

	public async Task<FixturesView?> GetFixtures(string TeamName = "") => await Client.GetFromJsonAsync<FixturesView?>($"api/tt/fixtures/{TeamName}");

	public async Task<Team?> GetTeam(string TeamName) => await Client.GetFromJsonAsync<Team?>($"api/tt/team/{TeamName}");

	public async Task<List<string>> GetTeamPlayers(string TeamName) => await Client.GetFromJsonAsync<List<string>?>($"api/tt/teamplayerslist/{TeamName}") ?? [];

}
