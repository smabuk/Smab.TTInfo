namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents the configuration settings for calculating division averages in a competition.
/// </summary>
/// <remarks>This record encapsulates various options and flags that influence how division averages are
/// calculated, including handling of forfeits, concessions, walkovers, and other specific rules.</remarks>
/// <param name="CompetitionId"></param>
/// <param name="SingleForfeit"></param>
/// <param name="SingleConcede"></param>
/// <param name="SingleWalkover"></param>
/// <param name="DoublesForfeit"></param>
/// <param name="DoublesConcede"></param>
/// <param name="DoublesWalkover"></param>
/// <param name="ExcludeReserves"></param>
/// <param name="CalculationType"></param>
/// <param name="OrderType"></param>
/// <param name="ExcludeNoWins"></param>
/// <param name="MinimumPlayed"></param>
/// <param name="ConsolidatedAverages"></param>
public sealed record DivisionAverages(
	int  CompetitionId,
	bool SingleForfeit,
	bool SingleConcede,
	bool SingleWalkover,
	bool DoublesForfeit,
	bool DoublesConcede,
	bool DoublesWalkover,
	bool ExcludeReserves,
	int  CalculationType,
	int  OrderType,
	bool ExcludeNoWins,
	int  MinimumPlayed,
	bool ConsolidatedAverages
);

