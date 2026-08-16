namespace Smab.TTInfo.Olop.Helpers;

public static partial class ExcelPackageHelpers
{
	extension(ExcelPackage package)
	{
		/// <summary>
		/// Gets the summary data from the "Summary" table in the "Summary" worksheet of the Excel package.
		/// </summary>
		/// <returns>An enumerable of <see cref="SummaryPlayerDetails"/> objects containing the summary data.</returns>
		public SummaryData GetSummaryData()
		{
			const int colRank = 0;
			const int colName = 1;
			//const int colWeeksPlayed = 2;
			const int colPercentage = 3;
			const int colPoints = 4;

			const int WEEKS_START = 6;
			const int WEEK_SUMMARY_START = 12;
			const int WEEK_SUMMARY_LENGTH = 14;

			ExcelWorksheet summarySheet = package.Workbook.Worksheets[SUMMARY_SHEET_NAME];
			ExcelTable summaryTable = package.Workbook.Worksheets[SUMMARY_SHEET_NAME].Tables[SUMMARY_TABLE_NAME];

			string title = summarySheet.Cells[1, 2].Text;
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

			Dictionary<NamedPlayer, SummaryPlayerDetails> summaryPlayerDetails = [];
			foreach (ExcelTableRow dataRow in summaryTable.DataRows) {
				ExcelRangeBase[] values = [.. dataRow.RowRange];
				string name = values[colName].Text;
				if (!string.IsNullOrWhiteSpace(name)) {
					int rank = Convert.ToInt32(values[colRank].Value);
					int weeksPlayed = validWeeks.Select(weekNo => string.IsNullOrWhiteSpace(values[WEEKS_START + weekNo - 2].Text) ? 0 : 1).Sum();
					double percentage = Convert.ToDouble(values[colPercentage].Value) * 100;
					double points = Convert.ToDouble(values[colPoints].Value);

					Dictionary<int, SummaryWeekDetails> weekDetails = [];
					for (int weekNo = 1; weekNo <= 6; weekNo++) {
						int colOffset = WEEK_SUMMARY_START + ((weekNo - 1) * WEEK_SUMMARY_LENGTH) - 1;
						int? MainByes = string.IsNullOrEmpty(values[colOffset + 0].Text) ? null : Convert.ToInt32(values[colOffset + 0].Value);
						int? Main32s = string.IsNullOrEmpty(values[colOffset + 1].Text) ? null : Convert.ToInt32(values[colOffset + 1].Value);
						int? Main16s = string.IsNullOrEmpty(values[colOffset + 2].Text) ? null : Convert.ToInt32(values[colOffset + 2].Value);
						int? MainQFs = string.IsNullOrEmpty(values[colOffset + 3].Text) ? null : Convert.ToInt32(values[colOffset + 3].Value);
						int? MainSFs = string.IsNullOrEmpty(values[colOffset + 4].Text) ? null : Convert.ToInt32(values[colOffset + 4].Value);
						int? MainFinals = string.IsNullOrEmpty(values[colOffset + 5].Text) ? null : Convert.ToInt32(values[colOffset + 5].Value);
						int? ConsolationByes = string.IsNullOrEmpty(values[colOffset + 0].Text) ? null : Convert.ToInt32(values[colOffset + 6].Value);
						int? Consolation16s = string.IsNullOrEmpty(values[colOffset + 7].Text) ? null : Convert.ToInt32(values[colOffset + 7].Value);
						int? ConsolationQFs = string.IsNullOrEmpty(values[colOffset + 8].Text) ? null : Convert.ToInt32(values[colOffset + 8].Value);
						int? ConsolationSFs = string.IsNullOrEmpty(values[colOffset + 9].Text) ? null : Convert.ToInt32(values[colOffset + 9].Value);
						int? ConsolationFinals = string.IsNullOrEmpty(values[colOffset + 10].Text) ? null : Convert.ToInt32(values[colOffset + 10].Value);
						int? TotalGamesWon = string.IsNullOrEmpty(values[colOffset + 11].Text) ? null : Convert.ToInt32(values[colOffset + 11].Value);
						int? TotalGamesLost = string.IsNullOrEmpty(values[colOffset + 12].Text) ? null : Convert.ToInt32(values[colOffset + 12].Value);
						double? WeekPoints = string.IsNullOrEmpty(values[colOffset + 13].Text) ? null : Convert.ToDouble(values[colOffset + 13].Value);

						SummaryWeekDetails summaryWeekDetails = new(
							WeekNo: weekNo,
							MainByes: MainByes,
							Main32sGamesWon: Main32s,
							Main16sGamesWon: Main16s,
							MainQuarterFinalGamesWon: MainQFs,
							MainSemiFinalGamesWon: MainSFs,
							MainFinalGamesWon: MainFinals,
							ConsolationByes: ConsolationByes,
							Consolation16sGamesWon: Consolation16s,
							ConsolationQuarterFinalGamesWon: ConsolationQFs,
							ConsolationSemiFinalGamesWon: ConsolationSFs,
							ConsolationFinalGamesWon: ConsolationFinals,
							GamesWon: TotalGamesWon,
							GamesLost: TotalGamesLost,
							PremierPoints: WeekPoints);
						weekDetails[weekNo] = summaryWeekDetails;
					}

					summaryPlayerDetails[new NamedPlayer(name)] = new(rank, new NamedPlayer(name), weeksPlayed, percentage, points, weekDetails);
				}
			}

			return new(title, validWeeks, summaryPlayerDetails);
		}
	}
}
