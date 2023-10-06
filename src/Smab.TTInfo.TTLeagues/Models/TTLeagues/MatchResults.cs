namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record MatchResults(
	IReadOnlyList<PlayerScore> Home,
	IReadOnlyList<PlayerScore> Away
);
