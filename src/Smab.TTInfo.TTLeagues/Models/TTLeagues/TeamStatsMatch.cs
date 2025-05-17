namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents the statistics of a team's performance in a match.
/// </summary>
/// <remarks>This record encapsulates key performance metrics for a team in a specific match, including the match
/// identifier, the number of matches won, the total matches played, the team's form, and the date of the
/// match.</remarks>
/// <param name="Id">The unique identifier of the match. Can be <see langword="null"/> if the match is not yet assigned an ID.</param>
/// <param name="Won">The number of matches won by the team. Can be <see langword="null"/> if the data is unavailable.</param>
/// <param name="Played">The total number of matches played by the team. Can be <see langword="null"/> if the data is unavailable.</param>
/// <param name="Form">A numeric representation of the team's recent performance or form. Can be <see langword="null"/> if the form data is
/// unavailable.</param>
/// <param name="Date">The date and time of the match. Can be <see langword="null"/> if the match date is not set.</param>
internal sealed record TeamStatsMatch(
	int? Id,
	int? Won,
	int? Played,
	int? Form,
	DateTimeOffset? Date
);
