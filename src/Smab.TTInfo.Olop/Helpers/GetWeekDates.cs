namespace Smab.TTInfo.Olop.Helpers;

public static partial class ExcelPackageHelpers
{
	const string WEEK_DATES_TABLE_NAME = "WeekDates";

	extension(ExcelPackage package)
	{
		/// <summary>
		/// Gets the week dates from the "WeekDates" table in the "Summary" worksheet of the Excel package.
		/// </summary>
		/// <returns>An enumerable of tuples containing the week number and the corresponding week date.</returns>
		public IEnumerable<WeekDates> GetWeekDates()
		{
			const int col_WeekNo = 0;
			const int col_WeekDate = 1;

			ExcelTable weekDatesTable = package.Workbook.Worksheets[SUMMARY_SHEET_NAME].Tables[WEEK_DATES_TABLE_NAME];

			foreach (ExcelTableRow dataRow in weekDatesTable.DataRows) {
				ExcelRangeBase[] values = [.. dataRow.RowRange];
				int weekNo = Convert.ToInt32(values[col_WeekNo].Value);
				DateOnly date = string.IsNullOrWhiteSpace(values[col_WeekDate].Text) ? DateOnly.MaxValue : DateOnly.Parse(values[col_WeekDate].Text);
				if (date <= DateOnly.FromDateTime(DateTime.Now)) {
					yield return new WeekDates(weekNo, date);
				}
			}
		}

		/// <summary>
		/// Gets the week date for a specific week number from the "WeekDates" table in the "Summary" worksheet of the Excel
		/// package.
		/// </summary>
		/// <param name="weekNo">The week number for which to get the week date.</param>
		/// <returns>The week date for the specified week number, or null if not found.</returns>
		public DateOnly? GetWeekDate(int weekNo)
		{
			const int COL_WEEK_NO = 0;
			const int COL_WEEK_DATE = 1;

			ExcelTable weekDatesTable = package.Workbook.Worksheets[SUMMARY_SHEET_NAME].Tables[WEEK_DATES_TABLE_NAME];

			foreach (ExcelTableRow dataRow in weekDatesTable.DataRows) {
				ExcelRangeBase[] values = [.. dataRow.RowRange];
				int weekNoValue = Convert.ToInt32(values[COL_WEEK_NO].Value);
				if (weekNoValue == weekNo) {
					return string.IsNullOrWhiteSpace(values[COL_WEEK_DATE].Text) ? null : DateOnly.Parse(values[COL_WEEK_DATE].Text);
				}
			}

			return null;
		}
	}
}
