namespace Smab.TTInfo.Olop.Models;

public record SummaryWeekDetails(
	int WeekNo,
	int? MainByes,
	int? Main32sGamesWon,
	int? Main16sGamesWon,
	int? MainQuarterFinalGamesWon,
	int? MainSemiFinalGamesWon,
	int? MainFinalGamesWon,
	int? ConsolationByes,
	int? Consolation16sGamesWon,
	int? ConsolationQuarterFinalGamesWon,
	int? ConsolationSemiFinalGamesWon,
	int? ConsolationFinalGamesWon,
	int? GamesWon,
	int? GamesLost,
	double? PremierPoints
);
