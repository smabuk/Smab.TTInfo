namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record DivisionAverages(
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

