namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

internal sealed record MatchSet(
	int?    Id,
	int?    MatchId,
	string  Scores,
	int?    HomeScore,
	int?    AwayScore,
	int?    Ordering,
	IReadOnlyList<MatchPlayer> Players,
	bool?   Fixed,
	IReadOnlyList<MatchGame>   Games,
	IReadOnlyList<MatchPlayer> HomePlayers,
	IReadOnlyList<MatchPlayer> AwayPlayers,
	int?    AwayId,
	int?    HomeId,
	DateTimeOffset? Completed,
	bool?   Locked
);
