namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record Fixtures
(
	IReadOnlyList<Group> Groups,
	IReadOnlyList<Match> Matches,
	long Type,
	long Total
);
