﻿using System.Text.RegularExpressions;

namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
	[GeneratedRegex("""(?<part1>.*)(?<year1>\d\d\d\d)(?<part2>.*)(?<year2>\d\d+)(?<part3>.*)""")]
	private static partial Regex SeasonRegex();

	/// <summary>
	/// 
	/// </summary>
	/// <param name="seasonId"></param>
	/// <param name="year"></param>
	/// <returns></returns>
	public string GetSeasonId(string seasonId, int year)
	{
		string newSeasonId = "";
		if (SeasonRegex().IsMatch(seasonId)) {
            System.Text.RegularExpressions.Match regexMatch = SeasonRegex().Match(seasonId);
			int year1 = regexMatch.Groups["year1"].Value.Length == 4 ? year : year % 100;
			int year2 = regexMatch.Groups["year2"].Value.Length == 4 ? year + 1 : (year + 1) % 100;
			newSeasonId = $"{regexMatch.Groups["part1"].Value}{year1}{regexMatch.Groups["part2"].Value}{year2}{regexMatch.Groups["part3"].Value}";
		}

		return newSeasonId;
	}
}
