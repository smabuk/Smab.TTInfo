namespace Smab.TTInfo.Olop.Models;

public record Week(int WeekNo, DateOnly? Date, List<WeekPlayerDetails> PlayerDetails, List<Round> Rounds);
