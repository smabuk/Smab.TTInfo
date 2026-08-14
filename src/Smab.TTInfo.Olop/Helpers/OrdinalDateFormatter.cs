namespace Smab.TTInfo.Olop.Helpers;

public static class OrdinalDateFormatterExtensions
{
	extension (DateOnly date)
	{
		public string ToOrdinalString()
		{
			int day = date.Day;
			string suffix = day switch
			{
				11 or 12 or 13 => "th",
				_ when day % 10 == 1 => "st",
				_ when day % 10 == 2 => "nd",
				_ when day % 10 == 3 => "rd",
				_ => "th"
			};

			return $"{day}{suffix} {date:MMMM yyyy}";

		}
	}

	extension (DateOnly? date)
	{
		public string ToOrdinalString() => date is null ? string.Empty : date.Value.ToOrdinalString();
	}

	extension (DateTime date)
	{
		public string ToOrdinalString() => DateOnly.FromDateTime(date).ToOrdinalString();
	}

	extension (DateTime? date)
	{
		public string ToOrdinalString() => date is null ? string.Empty : DateOnly.FromDateTime(date.Value).ToOrdinalString();
	}
}




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
			int day = date.Day;
			string suffix = day switch
			{
				11 or 12 or 13 => "th",
				_ when day % 10 == 1 => "st",
				_ when day % 10 == 2 => "nd",
				_ when day % 10 == 3 => "rd",
				_ => "th"
			};

			return $"{day}{suffix} {date:MMMM yyyy}";
		}

		// Fall back to default formatting
		if (arg is IFormattable formattable) {
			return formattable.ToString(format, formatProvider);
		}

		return arg.ToString() ?? string.Empty;
	}

	public object? GetFormat(Type? formatType) => formatType == typeof(ICustomFormatter) ? this : null;
}
