namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a match game with details about the teams, update timestamp, and ordering.
/// </summary>
/// <param name="Id">The unique identifier of the match game. Can be <see langword="null"/> if not assigned.</param>
/// <param name="Home">The identifier of the home team. Can be <see langword="null"/> if not assigned.</param>
/// <param name="Away">The identifier of the away team. Can be <see langword="null"/> if not assigned.</param>
/// <param name="Updated">The timestamp indicating when the match game was last updated. Can be <see langword="null"/> if not available.</param>
/// <param name="Ordering">The ordering value used to sort or prioritize the match game. Can be <see langword="null"/> if not specified.</param>
internal sealed record MatchGame(
	int? Id,
	int? Home,
	int? Away,
	DateTimeOffset? Updated,
	int? Ordering
);
