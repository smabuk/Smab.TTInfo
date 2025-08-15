namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents the standing of a team or entrant in a competition, including performance statistics and ranking
/// information.
/// </summary>
/// <remarks>This record encapsulates various metrics related to a team's performance, such as matches played,
/// wins, losses, and points. It is typically used to display or calculate standings in a competition
/// leaderboard.</remarks>
/// <param name="EntrantId"></param>
/// <param name="TeamId"></param>
/// <param name="FullName"></param>
/// <param name="Name"></param>
/// <param name="Position"></param>
/// <param name="Played"></param>
/// <param name="Won"></param>
/// <param name="Drawn"></param>
/// <param name="Lost"></param>
/// <param name="SetsFor"></param>
/// <param name="SetsAgainst"></param>
/// <param name="GamesFor"></param>
/// <param name="GamesAgainst"></param>
/// <param name="GamePointsFor"></param>
/// <param name="GamePointsAgainst"></param>
/// <param name="Adjustment"></param>
/// <param name="PointsAgainst"></param>
/// <param name="Points"></param>
/// <param name="CompetitionId"></param>
public sealed record TeamStanding(
	int     EntrantId,
	int     TeamId,
	string  FullName,
	string  Name,
	int     Position,
	int     Played,
	int     Won,
	int     Drawn,
	int     Lost,
	int     SetsFor,
	int     SetsAgainst,
	int     GamesFor,
	int     GamesAgainst,
	int     GamePointsFor,
	int     GamePointsAgainst,
	int     Adjustment,
	int     PointsAgainst,
	int     Points,
	int     CompetitionId
 );
