namespace Smab.TTInfo.Olop.Helpers;

public static partial class ExcelPackageHelpers
{
	extension(ExcelPackage package)
	{
		public IEnumerable<WeekPlayerDetails> GetWeekPlayerDetails(int weekNo)
		{
			ExcelTable playersTable = package.Workbook.Worksheets[$"Week{weekNo}"].Tables[$"PlayersWeek{weekNo}"] ?? throw new Exception($"Players table for week {weekNo} not found.");

			foreach (ExcelTableRow dataRow in playersTable.DataRows) {
				ExcelRangeBase[] values = [.. dataRow.RowRange];
				string name = values[0].Text;
				if (!string.IsNullOrWhiteSpace(name)) {
					NamedPlayer player = new(name);
					double points = Convert.ToDouble(values[1].Value);
					int gamesWon = Convert.ToInt32(values[2].Value);
					int gamesLost = Convert.ToInt32(values[3].Value);
					yield return new(player, points, gamesWon, gamesLost);
				}
			}

		}
	}
}
