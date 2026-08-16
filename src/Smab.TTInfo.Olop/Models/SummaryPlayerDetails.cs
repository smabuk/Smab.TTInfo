namespace Smab.TTInfo.Olop.Models;

public record SummaryPlayerDetails(
	int Rank,
	NamedPlayer Player,
	int WeeksPlayed,
	double PercentageGamesWon,
	double Points,
	Dictionary<int, SummaryWeekDetails> Weeks
);
