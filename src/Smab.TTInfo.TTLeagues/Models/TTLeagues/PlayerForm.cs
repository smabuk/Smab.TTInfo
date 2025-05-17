namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a player's form in TTLeagues, including match and performance details.
/// </summary>
/// <param name="MatchId">The match identifier.</param>
/// <param name="Versus">The opponent information.</param>
/// <param name="Won">The number of matches won.</param>
/// <param name="Played">The number of matches played.</param>
/// <param name="Form">The form value.</param>
/// <param name="Date">The date of the match.</param>
internal sealed record PlayerForm(
	int?     MatchId,
	IntKeyValue Versus,
	int?     Won,
	int?     Played,
	int?     Form,
	DateTimeOffset? Date
);
