namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record TeamStanding(
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
