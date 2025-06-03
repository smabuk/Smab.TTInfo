namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a collection of fixtures in TTLeagues, including groups and matches.
/// </summary>
/// <param name="Groups">The list of groups in the fixtures.</param>
/// <param name="Matches">The list of matches in the fixtures.</param>
/// <param name="Type">The type of fixtures.</param>
/// <param name="Total">The total number of fixtures.</param>
internal sealed record Fixtures
(
	ImmutableList<Group> Groups,
	ImmutableList<Match> Matches,
	long Type,
	long Total
);
