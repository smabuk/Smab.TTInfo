namespace Smab.TTInfo.Olop.Helpers;

public static partial class ExcelPackageHelpers
{
	extension(ExcelPackage package)
	{
		public Week GetWeek(int weekNo)
		{
			List<WeekPlayerDetails> playerDetails = [.. package.GetWeekPlayerDetails(weekNo)];
			List<Round> rounds = [.. package.GetRoundsForWeek(weekNo)];
			DateOnly? date = package.GetWeekDate(weekNo);
			return new Week(weekNo, date, playerDetails, rounds);
		}
	}
}
