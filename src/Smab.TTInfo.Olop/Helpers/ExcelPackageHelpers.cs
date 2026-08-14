namespace Smab.TTInfo.Olop.Helpers;

public static class ExcelPackageHelpers
{
	public static string SUMMARY_SHEET_NAME = "Summary";
	public static string SUMMARY_TABLE_NAME = "Summary";
	public static string WEEK_DATES_TABLE_NAME = "WeekDates";


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
		/// Gets the summary data from the "Summary" table in the "Summary" worksheet of the Excel package.
		/// </summary>
		/// <returns>
		/// An enumerable of tuples containing the rank, name, weeks played, percentage of games won, and points.
		/// </returns>
		public IEnumerable<SummaryTable> GetSummaryData()
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
					yield return new SummaryTable(rank, name, weeksPlayed, percentage, points);
				}
			}
		}

		public IEnumerable<Round> GetRoundsForWeek(int weekNo)
		{
			const int ROUND_COLS_OFFSET = 6; // Starting column for rounds
			const int MAIN_ROW = 3; // Starting row for main rounds
			const int CONSOLATION_ROW = 23; // Starting row for consolation rounds

			int[] startColumnsRounds = [6, 16, 26, 36, 46];
			int[] startColumnsRows = [MAIN_ROW, CONSOLATION_ROW];
			//List<string> roundNames = ["32s", "16s", "QUARTER FINAL", "SEMI FINAL", "FINAL"];

			ExcelWorksheet weekSheet = package.Workbook.Worksheets[$"Week{weekNo}"] ?? throw new Exception($"Worksheet for week {weekNo} not found.");

			foreach (int startRow in startColumnsRows) {
				for (int roundIndex = 0; roundIndex < startColumnsRounds.Length; roundIndex++) {
					int noOfMatches = startColumnsRounds[roundIndex] switch
					{
						6 => 16,
						16 => 8,
						26 => 4,
						36 => 2,
						46 => 1,
						_ => throw new Exception($"Unexpected main round column: {roundIndex}")
					};

					if (startRow == CONSOLATION_ROW && roundIndex is 0) {
						continue; // Skip the first round in the consolation bracket as it doesn't exist
					}

					string roundName = weekSheet.Cells[startRow - 2, startColumnsRounds[roundIndex]].Text;

					if (roundIndex == startColumnsRounds.Length - 1) { // FINAL is formatted differently
						int startCol = startColumnsRounds[roundIndex];
						List<Match> matches = [];
						for (int matchNo = 0; matchNo < noOfMatches; matchNo++) {
							Player player1 = weekSheet.Cells[startRow, startCol].Text.ToPlayer();
							Player player2 = weekSheet.Cells[startRow, startCol + 3].Text.ToPlayer();
							if (player1 is NoPlayer && player2 is NoPlayer) {
								continue; // Skip if both player names are empty
							}

							List<Game> games = [];
							if (player1 is NamedPlayer && player2 is NamedPlayer) {
								for (int gameIndex = 0; gameIndex < 5; gameIndex++) {
									string score1Text = weekSheet.Cells[startRow + gameIndex, startCol + 1].Text;
									string score2Text = weekSheet.Cells[startRow + gameIndex, startCol + 2].Text;
									if (!string.IsNullOrWhiteSpace(score1Text) && !string.IsNullOrWhiteSpace(score2Text)) {
										int score1 = Convert.ToInt32(score1Text);
										int score2 = Convert.ToInt32(score2Text);
										games = [.. games, new(score1, score2)];
									}
								}
							}

							_ = double.TryParse(weekSheet.Cells[startRow + 9, startCol + 6].Text, out double points1);
							_ = double.TryParse(weekSheet.Cells[startRow + 11, startCol + 6].Text, out double points2);

							if (player2.Name == weekSheet.Cells[startRow + 9, startCol + 3].Text) {
								(points1, points2) = (points2, points1); // Swap points if player2 is in the WINNERS row
							}

							Match match = new(player1, player2, games, points1, points2);
							matches = [.. matches, match];
						}

						if (matches is not []) {
							Round round = startRow switch
							{
								3 => new MainRound(roundIndex + 1, roundName, matches),
								_ => new ConsolationRound(roundIndex + 1, roundName, matches)
							};
							yield return round;
						}
					} else {
						List<Match> matches = [];
						for (int matchNo = 0; matchNo < noOfMatches; matchNo++) {
							Player player1 = weekSheet.Cells[startRow + (matchNo * 2), startColumnsRounds[roundIndex]].Text.ToPlayer();
							Player player2 = weekSheet.Cells[startRow + (matchNo * 2) + 1, startColumnsRounds[roundIndex]].Text.ToPlayer();
							if (player1 is NoPlayer && player2 is NoPlayer) {
								continue; // Skip if both player names are empty
							}

							List<Game> games = [];
							if (player1 is NamedPlayer && player2 is NamedPlayer) {
								for (int gameIndex = 0; gameIndex < 5; gameIndex++) {
									int colOffset = startColumnsRounds[roundIndex] + 1 + gameIndex;
									string score1Text = weekSheet.Cells[startRow + (matchNo * 2), colOffset].Text;
									string score2Text = weekSheet.Cells[startRow + (matchNo * 2) + 1, colOffset].Text;
									if (!string.IsNullOrWhiteSpace(score1Text) && !string.IsNullOrWhiteSpace(score2Text)) {
										int score1 = Convert.ToInt32(score1Text);
										int score2 = Convert.ToInt32(score2Text);
										games = [.. games, new(score1, score2)];
									}
								}
							}

							_ = double.TryParse(weekSheet.Cells[startRow + (matchNo * 2), startColumnsRounds[roundIndex] + 8].Text, out double points1);
							_ = double.TryParse(weekSheet.Cells[startRow + (matchNo * 2) + 1, startColumnsRounds[roundIndex] + 8].Text, out double points2);

							Match match = new(player1, player2, games, points1, points2);
							matches = [.. matches, match];
						}

						if (matches is not []) {
							Round round = startRow switch
							{
								3 => new MainRound(roundIndex + 1, roundName, matches),
								_ => new ConsolationRound(roundIndex + 1, roundName, matches)
							};
							yield return round;
						}
					}


				}

			}
		}
	}
}
