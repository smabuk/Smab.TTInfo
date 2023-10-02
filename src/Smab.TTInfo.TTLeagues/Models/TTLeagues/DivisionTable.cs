namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record DivisionTable(
	int  CompetitionId,
	bool WinLossDraw,
	bool SetsForAgainst,
	bool GamesForAgainst,
	bool PointsForAgainst,
	bool PointsAdjustments,
	bool SetsRatio,
	bool GamesRatio,
	bool PointsRatio,
	int  OrderType,
	bool SetAlternative
);

