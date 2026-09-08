namespace Smab.Shared.Helpers;

/// <summary>
/// Extension methods for <see cref="DateTime"/> to provide formatted string representations.
/// </summary>
public static class DateTimeExtensions
{
	internal static readonly string DD_MMMM_YYYY = "dd MMMM, yyyy";
	internal static readonly string GMT = "GMT Standard Time";

	extension(DateTime dateTime)
	{
		public DateTime ToUKTime()
			=> TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, GMT);

		public string ToUtcString() => dateTime.ToString("u").Replace(" ", "T");

		/// <summary>
		/// Converts a <see cref="DateTime"/> to a string in the format "dd MMMM, yyyy".
		/// </summary>
		public string ToDateLongMonthYearString() => dateTime.ToString(DD_MMMM_YYYY);
	}

	extension(DateTime? dateTime)
	{
		public DateTime? ToUKTime() => dateTime?.ToUKTime();

		public string? ToUtcString() => dateTime?.ToString("u").Replace(" ", "T");

		/// </summary>
		/// Converts a <see cref="DateTime?"/> to a UTC string representation.
		/// <summary>
		/// </summary>
		/// Converts a UTC <see cref="DateTime?"/> to UK local time.
		/// <summary>
		/// </summary>
		/// Converts a <see cref="DateTime"/> to a UTC string representation.
		/// <summary>
		/// </summary>
		/// Converts a UTC <see cref="DateTime"/> to UK local time.
		/// <summary>
		/// <summary>
		/// Converts a nullable <see cref="DateTime"/> to a string in the format "dd MMMM, yyyy" or an empty string if null.
		/// </summary>
		public string ToDateLongMonthYearString() => dateTime?.ToString(DD_MMMM_YYYY) ?? "";
	}

}
