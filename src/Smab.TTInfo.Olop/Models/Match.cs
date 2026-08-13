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
