namespace Smab.Shared.Helpers;

/// <summary>
/// Provides extension methods for the <see cref="DateTimeOffset"/> type.
/// </summary>
public static class DateTimeOffsetExtensions
{
	internal static readonly string DD_MMMM_YYYY = "dd MMMM, yyyy";
	internal static readonly string GMT = "GMT Standard Time";

	extension(DateTimeOffset dateTime)
	{
		/// <summary>
		/// Converts a UTC <see cref="DateTimeOffset"/> to UK local time.
		/// </summary>
		/// <returns>The UK local time equivalent of the UTC <see cref="DateTimeOffset"/>.</returns>
		public DateTime ToUKTime()
			=> TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime.UtcDateTime, GMT);

		/// <summary>
		/// Converts a <see cref="DateTimeOffset"/> to a UTC string representation.
		/// </summary>
		/// <returns>The UTC string representation of the <see cref="DateTimeOffset"/>.</returns>
		public string ToUtcString()
			=> dateTime.ToString("u").Replace(" ", "T");

		/// <summary>
		/// Converts a <see cref="DateTimeOffset"/> to a string in the format "dd MMMM, yyyy".
		/// </summary>
		/// <returns>The string representation of the <see cref="DateTimeOffset"/> in the format "dd MMMM, yyyy".</returns>
		public string ToDateLongMonthYearString() => dateTime.ToString(DD_MMMM_YYYY);
	}

	extension(DateTimeOffset? dateTime)
	{
		/// <summary>
		/// Converts a nullable UTC <see cref="DateTimeOffset"/> to UK local time, or null if null.
		/// </summary>
		/// <returns>The UK local time equivalent of the nullable UTC <see cref="DateTimeOffset"/>, or null if null.</returns>
		public DateTime? ToUKTime() => dateTime?.ToUKTime();

		/// <summary>
		/// Converts a nullable <see cref="DateTimeOffset"/> to a UTC string representation, or null if null.
		/// </summary>
		/// <returns>The UTC string representation of the nullable <see cref="DateTimeOffset"/>, or null if null.</returns>
		public string? ToUtcString() => dateTime?.ToUtcString();

		/// <summary>
		/// Converts a nullable <see cref="DateTimeOffset"/> to a string in the format "dd MMMM, yyyy", or an empty string if
		/// null.
		/// </summary>
		/// <returns>The string representation of the nullable <see cref="DateTimeOffset"/> in the format "dd MMMM, yyyy", or an empty string if null.</returns>
		public string ToDateLongMonthYearString() => dateTime?.ToDateLongMonthYearString() ?? "";
	}
}
