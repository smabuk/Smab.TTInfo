namespace Smab.TTInfo.Olop.Helpers;

public static partial class ExcelPackageHelpers
{
	extension(ExcelPackage package)
	{
		public IEnumerable<WeekPlayerDetails> GetWeekPlayerDetails(int weekNo)
		{
			ExcelTable playersTable = package.Workbook.Worksheets[$"Week{weekNo}"].Tables[$"PlayersWeek{weekNo}"] ?? throw new Exception($"Players table for week {weekNo} not found.");

			List<WeekPlayerDetails> playerDetails = [];
			foreach (ExcelTableRow dataRow in playersTable.DataRows) {
				ExcelRangeBase[] values = [.. dataRow.RowRange];
				string name = values[0].Text;
				if (!string.IsNullOrWhiteSpace(name)) {
					NamedPlayer player = new(name);
					double points = Convert.ToDouble(values[1].Value);
					int gamesWon = Convert.ToInt32(values[2].Value);
					int gamesLost = Convert.ToInt32(values[3].Value);
					playerDetails.Add(new(player, points, gamesWon, gamesLost));
				}
			}

			List<WeekPlayerDetails> rankedPlayerDetails = [.. playerDetails
				.OrderByDescending(pd => pd.PremierPoints)
				.Select((pd, index) => new { pd, index })
				.GroupBy(x => x.pd.PremierPoints)
				.SelectMany(g =>
				{
					int rank = g.Min(x => x.index) + 1;
					return g.Select(x => x.pd with { Rank = rank });
				})];

			return rankedPlayerDetails;
		}
	}
}
