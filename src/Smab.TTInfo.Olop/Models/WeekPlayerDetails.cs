namespace Smab.TTInfo.Olop.Models;

public record WeekPlayerDetails(
	NamedPlayer Player,
	double PremierPoints,
	int GamesWon,
	int GamesLost
);

public static partial class WeekPlayerDetailsExtensions
{
	extension(WeekPlayerDetails)
	{
		//public static WeekPlayerDetails CreateWithoutSummaryDetails(NamedPlayer player, double premierPoints, int gamesWon, int gamesLost) => new(
		//	Player: player,
		//	PremierPoints: premierPoints,
		//	GamesWon: gamesWon,
		//	GamesLost: gamesLost,
		//	MainByes: null,
		//	Main32sGamesWon: null,
		//	Main16sGamesWon: null,
		//	MainQuarterFinalGamesWon: null,
		//	MainSemiFinalGamesWon: null,
		//	MainFinalGamesWon: null,
		//	ConsolationByes: null,
		//	Consolation32sGamesWon: null,
		//	Consolation16sGamesWon: null,
		//	ConsolationQuarterFinalGamesWon: null,
		//	ConsolationSemiFinalGamesWon: null,
		//	ConsolationFinalGamesWon: null
		//);
	}
}
