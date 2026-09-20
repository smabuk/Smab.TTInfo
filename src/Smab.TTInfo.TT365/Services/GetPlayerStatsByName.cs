using System.Transactions;

namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
	/// <summary>
	/// Retrieves and updates the statistics for a specified player in a given league and season.
	/// </summary>
	/// <remarks>
	/// This method fetches player statistics from a remote source, processes the data, and updates the player's
	/// information. If the player's statistics are already cached, the cached data is used. If the player's URL is not
	/// set, it is generated based on the league and season information. The method also saves the updated player data to a
	/// local cache for future use.
	/// </remarks>
	/// <param name="leagueId">The unique identifier of the league to retrieve data from.</param>
	/// <param name="player">The player whose statistics are to be retrieved and updated.</param>
	/// <param name="seasonId">
	/// The unique identifier of the season. If not provided or empty, the current season of the league will be used.
	/// </param>
	/// <returns>
	/// A <see cref="Player"/> object containing the updated statistics for the specified player, or <see langword="null"/>
	/// if the league cannot be found.
	/// </returns>
	public async Task<Player?> GetPlayerStatsByName(TT365LeagueId leagueId, string playerName, TT365SeasonId? seasonId = null)
	{
		League? league = await GetLeague(leagueId);
		if (league is null) { return null; }

		seasonId ??= league.CurrentSeasonId;
		if (seasonId is null) { return null; }

		playerName = playerName.Replace("%20", " ").Replace("_", " ");

		// ToDo: Implement AKA logic for players known by more than one name in the TT365 system
		List<List<string>> playerNameToAKA =
		[
			["Jon Abbott", "Jonathan Abbott"],
			["Kash Subhan", "Kashif Subhan", "K Subhan"],
			["Mike Childs", "Michael Childs"],
		];

		List<string> akaNames = playerNameToAKA.FirstOrDefault(akaList => akaList.Contains(playerName, StringComparer.OrdinalIgnoreCase)) ?? [playerName];

		List<Fixture>? fixtures = await GetAllFixtures(leagueId, seasonId);
		Player? player = fixtures.OfType<CompletedFixture>()
			.SelectMany(f => f.HomePlayers.Concat(f.AwayPlayers))
			.Where(p => akaNames.Contains(p.Name, StringComparer.OrdinalIgnoreCase))
			.Select(p => new Player { Name = p.Name, PlayerId = p.Id })
			.FirstOrDefault();

		if (player is null || player.Id == 0) { return null; }

		Player? newPlayer =  await GetPlayerStats(leagueId, player, seasonId);
		return newPlayer;
	}
}

