namespace Smab.TTInfo.TT365.Helpers;

internal static class DateHelpers
{
	extension(DateOnly)
	{
		public static bool TryParseWithMissingYear(string partialDate, int startYear, out DateOnly fullDate)
		{
			string fullerDate = partialDate switch
			{
				_ when partialDate.Contains("Sep") || partialDate.Contains("Oct") || partialDate.Contains("Nov") || partialDate.Contains("Dec") => $"{partialDate} {startYear}",
				_ => $"{partialDate} {startYear + 1}",
			};

			bool success = DateOnly.TryParse(fullerDate, out fullDate);
			return success;
		}
	}
}
