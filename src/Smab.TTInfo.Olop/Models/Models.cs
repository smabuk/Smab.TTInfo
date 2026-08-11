namespace Smab.TTInfo.Olop.Models;

public record SummaryTable(int Rank, string Player, int WeeksPlayed, double PercentageGamesWon, double Points);
public record WeekDates(int WeekNo, DateOnly WeekDate);
