namespace Smab.TTInfo.TTLeagues.Models;
internal sealed record MatchCard(
	int   Id,
	Match Match,
	MatchResults Results,
	IReadOnlyList<MatchSet> Sets
);
