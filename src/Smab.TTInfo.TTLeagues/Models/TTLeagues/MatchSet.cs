namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a set of matches, including scores, players, games, and related metadata.
/// </summary>
/// <remarks>This record encapsulates the details of a match set, including scores, participating players,
/// associated games, and various flags indicating the state of the match set (e.g., whether it is fixed, completed, or
/// locked). It is designed to provide a comprehensive view of a match set's data in a read-only manner.</remarks>
/// <param name="Id"></param>
/// <param name="MatchId"></param>
/// <param name="Scores"></param>
/// <param name="HomeScore"></param>
/// <param name="AwayScore"></param>
/// <param name="Ordering"></param>
/// <param name="Players"></param>
/// <param name="Fixed"></param>
/// <param name="Games"></param>
/// <param name="HomePlayers"></param>
/// <param name="AwayPlayers"></param>
/// <param name="AwayId"></param>
/// <param name="HomeId"></param>
/// <param name="Completed"></param>
/// <param name="Locked"></param>
public sealed record MatchSet(
	int?    Id,
	int?    MatchId,
	string  Scores,
	int?    HomeScore,
	int?    AwayScore,
	int?    Ordering,
	ImmutableList<MatchPlayer> Players,
	bool?   Fixed,
	ImmutableList<MatchGame>   Games,
	ImmutableList<MatchPlayer> HomePlayers,
	ImmutableList<MatchPlayer> AwayPlayers,
	int?    AwayId,
	int?    HomeId,
	DateTimeOffset? Completed,
	bool?   Locked
);
