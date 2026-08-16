namespace Smab.TTInfo.Olop.Models;

public record Week(int WeekNo, DateOnly? Date, List<WeekPlayerDetails> PlayerDetails, List<Round> Rounds);

public static class WeekExtensions
{
	extension(Week week)
	{
		public WeekPlayerDetails? GetPlayerDetails(NamedPlayer player) => week.PlayerDetails.FirstOrDefault(pd => pd.Player == player);
	}
}
