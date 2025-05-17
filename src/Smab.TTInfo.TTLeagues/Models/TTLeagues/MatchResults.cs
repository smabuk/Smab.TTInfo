namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents the results of a match, including the scores of players on both the home and away teams.
/// </summary>
/// <param name="Home">A read-only list of player scores for the home team. Each entry represents an individual player's score.</param>
/// <param name="Away">A read-only list of player scores for the away team. Each entry represents an individual player's score.</param>
internal sealed record MatchResults(
	IReadOnlyList<PlayerScore> Home,
	IReadOnlyList<PlayerScore> Away
);
