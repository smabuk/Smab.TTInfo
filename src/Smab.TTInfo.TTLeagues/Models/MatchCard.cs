namespace Smab.TTInfo.TTLeagues.Models;

/// <summary>
/// Represents a card containing information about a match, its results, and associated sets.
/// </summary>
/// <remarks>This record encapsulates the details of a match, including its unique identifier, the match data, the
/// results of the match, and a collection of sets associated with the match.</remarks>
/// <param name="Id">The unique identifier for the match card.</param>
/// <param name="Match">The match associated with this card.</param>
/// <param name="Results">The results of the match.</param>
/// <param name="Sets">A read-only list of sets associated with the match.</param>
public sealed record MatchCard(
	int   Id,
	Match Match,
	MatchResults Results,
	ImmutableList<MatchSet> Sets
);
