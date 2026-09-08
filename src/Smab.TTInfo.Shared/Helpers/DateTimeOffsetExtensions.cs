namespace Smab.Shared.Helpers;

/// <summary>
/// Extension methods for <see cref="DateTimeOffset"/> to provide UK time and formatted string representations.
/// </summary>
public static class DateTimeOffsetExtensions
{
	internal static readonly string DD_MMMM_YYYY = "dd MMMM, yyyy";
	internal static readonly string GMT = "GMT Standard Time";

	extension(DateTimeOffset dateTimeUtc)
	{
		/// <summary>
		/// Converts a UTC <see cref="DateTimeOffset"/> to UK local time.
		/// </summary>
		public DateTime ToUKTime()
			=> TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTimeUtc.UtcDateTime, GMT);

		/// <summary>
		/// Converts a <see cref="DateTimeOffset"/> to a UTC string representation.
		/// </summary>
		public string ToUtcString()
			=> dateTimeUtc.ToString("u").Replace(" ", "T");
	}

	extension(DateTimeOffset? dateTimeUtc)
	{
		/// <summary>
		/// Converts a nullable UTC <see cref="DateTimeOffset"/> to UK local time, or null if null.
		/// </summary>
		public DateTime? ToUKTime()
			=> dateTimeUtc switch
			{
				not null => TimeZoneInfo.ConvertTimeBySystemTimeZoneId(((DateTimeOffset)dateTimeUtc).UtcDateTime, GMT),
				null => null
			};

		/// <summary>
		/// Converts a nullable <see cref="DateTimeOffset"/> to a UTC string representation, or null if null.
		/// </summary>
		public string? ToUtcString()
			=> dateTimeUtc switch
			{
				not null => ((DateTimeOffset)dateTimeUtc).ToUtcString(),
				null => null
			};
	}
}
