namespace Smab.TTInfo.Olop.Models;

public record Match(Player Player1, Player Player2, List<Game> Games, double PremierPoints1, double PremierPoints2)
{
		public override string ToString()
		{
			return (Player1, Player2) switch
			{
				(NoPlayer, NoPlayer) => "No match",
				(NamedPlayer player1, NoPlayer) => $"{player1.Name} vs unknown",
				(NoPlayer, NamedPlayer player2) => $"unknown vs {player2.Name}",
				(NamedPlayer player1, Bye) => $"{player1.Name} ({PremierPoints1:F1}) has a bye",
				(Bye, NamedPlayer player2) => $"{player2.Name} ({PremierPoints2:F1}) has a bye",
				(Bye, Bye) => "Bye",
				_ => $"{Player1.Name} ({PremierPoints1:F1}) vs {Player2.Name} ({PremierPoints2:F1}): {string.Join(", ", Games.Select(g => $"{g}"))}"
			};

		}
};

public static class MatchExtensions
{
	const int NO_OF_GAMES_TO_WIN = 3;
	extension(Match match)
	{
		public bool IsBye => match.Player1 is Bye || match.Player2 is Bye;
		public bool IsNoMatch => match.Player1 is NoPlayer && match.Player2 is NoPlayer;
		public bool Player1IsWinner => match.Games.Count(g => g.Points1 > g.Points2) == NO_OF_GAMES_TO_WIN;
		public bool Player2IsWinner => match.Games.Count(g => g.Points2 > g.Points1) == NO_OF_GAMES_TO_WIN;
	}
}
