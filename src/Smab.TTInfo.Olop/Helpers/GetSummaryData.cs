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

			const int WEEKS_START = 6;

			ExcelWorksheet summarySheet = package.Workbook.Worksheets[SUMMARY_SHEET_NAME];
			ExcelTable summaryTable = package.Workbook.Worksheets[SUMMARY_SHEET_NAME].Tables[SUMMARY_TABLE_NAME];

			List<int> validWeeks = [];
			for (int weekNo = 1; weekNo <= 6; weekNo++) {
				bool foundNonZero = false;
				for (int i = 0; i < summaryTable.DataRows.Count(); i++) {
					int rowIdx = i + 4; // Data rows start at row
					string ptsString = summarySheet.Cells[rowIdx, WEEKS_START + weekNo - 1].Text;
					if (string.IsNullOrWhiteSpace(ptsString)) {
						break;
					}

					_ = double.TryParse(ptsString, out double weekPoints);
					if (weekPoints != 0.0) {
						foundNonZero = true;
						break;
					}
				}

				if (foundNonZero is true) {
					validWeeks.Add(weekNo);
				}
			}

			foreach (ExcelTableRow dataRow in summaryTable.DataRows) {
				ExcelRangeBase[] values = [.. dataRow.RowRange];
				string name = values[colName].Text;
				if (!string.IsNullOrWhiteSpace(name)) {
					int rank = Convert.ToInt32(values[colRank].Value);
					int weeksPlayed = validWeeks.Select(weekNo => string.IsNullOrWhiteSpace(values[WEEKS_START + weekNo - 2].Text) ? 0 : 1).Sum();
					double percentage = Convert.ToDouble(values[colPercentage].Value) * 100;
					double points = Convert.ToDouble(values[colPoints].Value);
					yield return new SummaryTableItem(rank, new NamedPlayer(name), weeksPlayed, percentage, points);
				}
			}
		}
	}
}
