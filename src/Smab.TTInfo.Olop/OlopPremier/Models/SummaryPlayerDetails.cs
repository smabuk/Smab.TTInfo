namespace Smab.TTInfo.Olop.OlopPremier.Models;

public record SummaryPlayerDetails(
	int Rank,
	NamedPlayer Player,
	int WeeksPlayed,
	double PercentageGamesWon,
	double Points,
	Dictionary<int, SummaryWeekDetails> Weeks
);
