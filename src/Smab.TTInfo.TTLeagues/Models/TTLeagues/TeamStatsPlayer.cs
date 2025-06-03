namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a player in team statistics, including matches and averages.
/// </summary>
/// <param name="Id">The unique identifier for the player.</param>
/// <param name="Matches">The list of matches played by the player.</param>
/// <param name="Average">The average score of the player.</param>
/// <param name="Name">The name of the player.</param>
/// <param name="Played">The number of matches played.</param>
/// <param name="Won">The number of matches won.</param>
/// <param name="Potm">The number of player of the match awards.</param>
internal sealed record TeamStatsPlayer(
	string Id,
	ImmutableList<TeamStatsMatch> Matches,
	double Average,
	string Name,
	int    Played,
	int    Won,
	int    Potm
);
