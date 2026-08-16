namespace Smab.TTInfo.Olop.Helpers;

public static partial class ExcelPackageHelpers
{
	extension(ExcelPackage package)
	{
		/// <summary>
		/// Gets the rounds for a specific week from the Excel package.
		/// </summary>
		/// <param name="weekNo">The week number for which to retrieve the rounds.</param>
		/// <returns>An enumerable of Round objects representing the rounds for the specified week.</returns>
		/// <exception cref="Exception">Thrown if the worksheet for the specified week is not found.</exception>
		public IEnumerable<Round> GetRoundsForWeek(int weekNo)
		{
			const int MAIN_ROW = 3; // Starting row for main rounds
			const int CONSOLATION_ROW = 23; // Starting row for consolation rounds

			int[] startColumnsRounds = [6, 16, 26, 36, 46];
			int[] startColumnsRows = [MAIN_ROW, CONSOLATION_ROW];

			ExcelWorksheet weekSheet = package.Workbook.Worksheets[$"Week{weekNo}"] ?? throw new Exception($"Worksheet for week {weekNo} not found.");

			foreach (int startRow in startColumnsRows) {
				foreach (RoundType roundType in Enum.GetValues<RoundType>()) {
					int noOfMatches = roundType.ToMatchCount();

					if (startRow == CONSOLATION_ROW && roundType is RoundType.Roundof32) {
						continue; // Skip the first round in the consolation bracket as it doesn't exist
					}

					string roundName = weekSheet.Cells[startRow - 2, startColumnsRounds[roundType.ToRoundNo() - 1]].Text;

					if (roundType is RoundType.Final) { // FINAL is formatted differently
						int startCol = startColumnsRounds[roundType.ToRoundNo() - 1];
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
								3 => new MainRound(roundType, roundName, matches),
								_ => new ConsolationRound(roundType, roundName, matches)
							};
							yield return round;
						}
					} else {
						List<Match> matches = [];
						for (int matchNo = 0; matchNo < noOfMatches; matchNo++) {
							Player player1 = weekSheet.Cells[startRow + (matchNo * 2), startColumnsRounds[roundType.ToRoundNo() - 1]].Text.ToPlayer();
							Player player2 = weekSheet.Cells[startRow + (matchNo * 2) + 1, startColumnsRounds[roundType.ToRoundNo() - 1]].Text.ToPlayer();
							if (player1 is NoPlayer && player2 is NoPlayer) {
								continue; // Skip if both player names are empty
							}

							List<Game> games = [];
							if (player1 is NamedPlayer && player2 is NamedPlayer) {
								for (int gameIndex = 0; gameIndex < 5; gameIndex++) {
									int colOffset = startColumnsRounds[roundType.ToRoundNo() - 1] + 1 + gameIndex;
									string score1Text = weekSheet.Cells[startRow + (matchNo * 2), colOffset].Text;
									string score2Text = weekSheet.Cells[startRow + (matchNo * 2) + 1, colOffset].Text;
									if (!string.IsNullOrWhiteSpace(score1Text) && !string.IsNullOrWhiteSpace(score2Text)) {
										int score1 = Convert.ToInt32(score1Text);
										int score2 = Convert.ToInt32(score2Text);
										games = [.. games, new(score1, score2)];
									}
								}
							}

							_ = double.TryParse(weekSheet.Cells[startRow + (matchNo * 2), startColumnsRounds[roundType.ToRoundNo() - 1] + 8].Text, out double points1);
							_ = double.TryParse(weekSheet.Cells[startRow + (matchNo * 2) + 1, startColumnsRounds[roundType.ToRoundNo() - 1] + 8].Text, out double points2);

							Match match = new(player1, player2, games, points1, points2);
							matches = [.. matches, match];
						}

						if (matches is not []) {
							Round round = startRow switch
							{
								3 => new MainRound(roundType, roundName, matches),
								_ => new ConsolationRound(roundType, roundName, matches)
							};
							yield return round;
						}
					}


				}
			}
		}
	}
}
