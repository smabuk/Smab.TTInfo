namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

internal sealed record MatchGame(
	int? Id,
	int? Home,
	int? Away,
	DateTimeOffset? Updated,
	int? Ordering
);
