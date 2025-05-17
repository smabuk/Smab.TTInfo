using System.Net.Http.Json;

namespace Smab.TTInfo.TT365;

/// <summary>
/// Client for accessing TT365 API endpoints for fixtures and team data.
/// </summary>
public sealed class TTInfoClient(HttpClient httpClient)
{
	/// <summary>
	/// Gets the underlying <see cref="HttpClient"/> instance.
	/// </summary>
	public HttpClient Client { get; } = httpClient;

	/// <summary>
	/// Gets fixtures for a team from the API.
	/// </summary>
	public async Task<FixturesView?> GetFixtures(string TeamName = "") => await Client.GetFromJsonAsync<FixturesView?>($"api/tt/fixtures/{TeamName}");

	/// <summary>
	/// Gets team details from the API.
	/// </summary>
	public async Task<Team?> GetTeam(string TeamName) => await Client.GetFromJsonAsync<Team?>($"api/tt/team/{TeamName}");

	/// <summary>
	/// Gets a list of player names for a team from the API.
	/// </summary>
	public async Task<List<string>> GetTeamPlayers(string TeamName) => await Client.GetFromJsonAsync<List<string>?>($"api/tt/teamplayerslist/{TeamName}") ?? [];

}
