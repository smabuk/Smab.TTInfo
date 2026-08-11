using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace Smab.TTInfo.Olop.Helpers;

public static class ExcelPackageHelpers
{
	extension(string oneDriveExcelLink)
	{
		public async Task<ExcelPackage> OpenExcelPackage(HttpClient httpClient)
		{
			using HttpResponseMessage response = await httpClient.GetAsync($"{oneDriveExcelLink}&download=1");

			if (response.IsSuccessStatusCode) {
				Stream stream = await response.Content.ReadAsStreamAsync();

				ExcelPackage.License.SetNonCommercialPersonal("Simon Brookes");
				return new ExcelPackage(stream);
			} else {
				throw new Exception($"Failed to download Excel file from {oneDriveExcelLink}. Status code: {response.StatusCode}");
			}

		}
	}

	extension(ExcelPackage package)
	{
		/// <summary>
		/// Gets the week dates from the "WeekDates" table in the "Summary" worksheet of the Excel package.
		/// </summary>
		/// <returns>An enumerable of tuples containing the week number and the corresponding week date.</returns>
		public IEnumerable<WeekDates> GetWeekDates()
		{
			ExcelTable weekDatesTable = package.Workbook.Worksheets["Summary"].Tables["WeekDates"];

			foreach (ExcelTableRow dataRow in weekDatesTable.DataRows) {
				ExcelRangeBase[] values = [.. dataRow.RowRange];
				int weekNo = Convert.ToInt32(values[0].Value);
				DateOnly date = string.IsNullOrWhiteSpace(values[1].Text) ? DateOnly.MaxValue : DateOnly.Parse(values[1].Text);
				if (date <= DateOnly.FromDateTime(DateTime.Now)) {
					yield return new WeekDates(weekNo, date);
				}
			}
		}

		/// <summary>
		/// Gets the summary data from the "Summary" table in the "Summary" worksheet of the Excel package.
		/// </summary>
		/// <returns>An enumerable of tuples containing the rank, name, weeks played, percentage of games won, and points.</returns>
		public IEnumerable<SummaryTable> GetSummaryData()
		{
			ExcelTable summaryTable = package.Workbook.Worksheets["Summary"].Tables["Summary"];
			foreach (ExcelTableRow dataRow in summaryTable.DataRows) {
				ExcelRangeBase[] values = [.. dataRow.RowRange];
				string name = values[1].Text;
				if (!string.IsNullOrWhiteSpace(name)) {
					int rank = Convert.ToInt32(values[0].Value);
					int weeksPlayed = Convert.ToInt32(values[2].Value);
					double percentage = Convert.ToDouble(values[3].Value) * 100;
					double points = Convert.ToDouble(values[4].Value);
					yield return new SummaryTable(rank, name, weeksPlayed, percentage, points);
				}
			}
		}
	}
}
