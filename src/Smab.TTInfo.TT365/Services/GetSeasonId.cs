using System.Text.RegularExpressions;

namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
	[GeneratedRegex("""(?<part1>.*)(?<year1>\d\d\d\d)(?<part2>.*)(?<year2>\d\d+)(?<part3>.*)""")]
	private static partial Regex SeasonRegex();

	/// <summary>
	/// Generates a <see cref="TT365SeasonId"/> based on the provided season identifier and year.
	/// </summary>
	/// <remarks>The method processes the input <paramref name="seasonIdAsString"/> to replace placeholders for year values
	/// with the actual year and the subsequent year based on the provided <paramref name="year"/>. If the input does not
	/// match the expected format, the returned season identifier will be empty.</remarks>
	/// <param name="seasonIdAsString">The original season identifier, which may include placeholders for year values.</param>
	/// <param name="year">The base year used to calculate the season's start and end years. Must be a valid integer representing a calendar
	/// year.</param>
	/// <returns>A <see cref="TT365SeasonId"/> object containing the updated season identifier with resolved year values.</returns>
	public TT365SeasonId GetSeasonId(string seasonIdAsString, int year)
	{
		string newSeasonId = "";
		if (SeasonRegex().IsMatch(seasonIdAsString)) {
            System.Text.RegularExpressions.Match regexMatch = SeasonRegex().Match(seasonIdAsString);
			int year1 = regexMatch.Groups["year1"].Value.Length == 4 ? year : year % 100;
			int year2 = regexMatch.Groups["year2"].Value.Length == 4 ? year + 1 : (year + 1) % 100;
			newSeasonId = $"{regexMatch.Groups["part1"].Value}{year1}{regexMatch.Groups["part2"].Value}{year2}{regexMatch.Groups["part3"].Value}";
		}

		return new(newSeasonId);
	}
}
