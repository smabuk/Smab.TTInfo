namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents the configuration settings for a division table in a competition.
/// </summary>
/// <remarks>This record encapsulates various options and parameters that define how a division table is
/// structured and calculated, including scoring methods, ratio calculations, and ordering preferences.</remarks>
/// <param name="CompetitionId"></param>
/// <param name="WinLossDraw"></param>
/// <param name="SetsForAgainst"></param>
/// <param name="GamesForAgainst"></param>
/// <param name="PointsForAgainst"></param>
/// <param name="PointsAdjustments"></param>
/// <param name="SetsRatio"></param>
/// <param name="GamesRatio"></param>
/// <param name="PointsRatio"></param>
/// <param name="OrderType"></param>
/// <param name="SetAlternative"></param>
public sealed record DivisionTable(
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

