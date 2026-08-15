using System.Text.RegularExpressions;

namespace Smab.TTInfo.Olop.Helpers;

public static partial class OrdinalDateFormatterExtensions
{
	const string DEFAULT_FORMAT = "d MMMM yyyy";

	/// <summary>
	/// Converts an integer to its ordinal string representation (e.g., 1 -> "1st", 2 -> "2nd", 3 -> "3rd", 4 -> "4th").
	/// </summary>
	/// <param name="number">The integer to convert to an ordinal string.</param>
	extension(int number)
	{
		public string ToOrdinalString()
		{
			string suffix = number switch
			{
				11 or 12 or 13 => "th",
				_ when number % 10 == 1 => "st",
				_ when number % 10 == 2 => "nd",
				_ when number % 10 == 3 => "rd",
				_ => "th"
			};

			return $"{number}{suffix}";
		}
	}

	/// <summary>
	/// Creates a regex to match "d" or "dd" but not "ddd" or "dddd".
	/// </summary>
	/// <returns>A Regex object that matches "d" or "dd" but not "ddd" or "dddd".</returns>
	[GeneratedRegex(@"(?<!d)d{1,2}(?!d)")]
	private static partial Regex Regex_d_dd();

	extension(DateOnly date) {
		/// <summary>
		/// Converts a DateOnly object to its ordinal string representation based on the specified format.
		/// </summary>
		/// <param name="format">The format string to use for the date.</param>
		/// <returns>The ordinal string representation of the date.</returns>
		public string ToOrdinalString(string format = DEFAULT_FORMAT)
		{
			string result = Regex_d_dd().Replace(format, "***");
			return date.ToString(result).Replace("***", date.Day.ToOrdinalString());
		}

	}
	extension(DateOnly? date) { public string ToOrdinalString(string format = DEFAULT_FORMAT) => date is null ? string.Empty : $"{date.Value.ToOrdinalString(format)}"; }
	extension(DateTime date) { public string ToOrdinalString(string format = DEFAULT_FORMAT) => $"{date.ToOrdinalString(format)}"; }
	extension(DateTime? date) { public string ToOrdinalString(string format = DEFAULT_FORMAT) => date is null ? string.Empty : $"{date.Value.ToOrdinalString(format)}"; }
}

/// <summary>
/// Provides custom formatting for DateOnly and DateTime objects to display dates in an ordinal format (e.g., "1st
/// January 2024").
/// </summary>
public class OrdinalDateFormatter : IFormatProvider, ICustomFormatter
{
	public string Format(string? format, object? arg, IFormatProvider? formatProvider)
	{
		// Handle null arg
		if (arg is null) {
			return string.Empty;
		}

		DateOnly date = arg switch
		{
			DateOnly d => d,
			DateTime dt => DateOnly.FromDateTime(dt),
			_ => throw new ArgumentException("Argument must be a DateOnly or DateTime.", nameof(arg))
		};

		// Check if this is an ordinal date format request
		if (format is "O" or "o") {
			return $"{date.Day.ToOrdinalString()} {date:MMMM yyyy}";
		}

		// Fall back to default formatting
		if (arg is IFormattable formattable) {
			return formattable.ToString(format, formatProvider);
		}

		return arg.ToString() ?? string.Empty;
	}

	public object? GetFormat(Type? formatType) => formatType == typeof(ICustomFormatter) ? this : null;
}
