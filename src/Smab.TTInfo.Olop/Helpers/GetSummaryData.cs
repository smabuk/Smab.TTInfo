namespace Smab.TTInfo.Olop.Helpers;

public static partial class ExcelPackageHelpers
{
	extension(ExcelPackage package)
	{
		/// <summary>
		/// Gets the summary data from the "Summary" table in the "Summary" worksheet of the Excel package.
		/// </summary>
		/// <returns>An enumerable of <see cref="SummaryTableItem"/> objects containing the summary data.</returns>
		public IEnumerable<SummaryTableItem> GetSummaryData()
		{
			const int colRank = 0;
			const int colName = 1;
			const int colWeeksPlayed = 2;
			const int colPercentage = 3;
			const int colPoints = 4;

			ExcelTable summaryTable = package.Workbook.Worksheets[SUMMARY_SHEET_NAME].Tables[SUMMARY_TABLE_NAME];

			foreach (ExcelTableRow dataRow in summaryTable.DataRows) {
				ExcelRangeBase[] values = [.. dataRow.RowRange];
				string name = values[colName].Text;
				if (!string.IsNullOrWhiteSpace(name)) {
					int rank = Convert.ToInt32(values[colRank].Value);
					int weeksPlayed = Convert.ToInt32(values[colWeeksPlayed].Value);
					double percentage = Convert.ToDouble(values[colPercentage].Value) * 100;
					double points = Convert.ToDouble(values[colPoints].Value);
					yield return new SummaryTableItem(rank, new NamedPlayer(name), weeksPlayed, percentage, points);
				}
			}
		}
	}
}
