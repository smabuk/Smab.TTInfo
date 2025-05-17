namespace Smab.Shared.Helpers;

/// <summary>
/// Extension methods for <see cref="DateTimeOffset"/> to provide UK time and formatted string representations.
/// </summary>
public static class DateTimeOffsetExtensions
{
	internal static readonly string DD_MMMM_YYYY = "dd MMMM, yyyy";

	/// <summary>
	/// Converts a UTC <see cref="DateTimeOffset"/> to UK local time.
	/// </summary>
	public static DateTime ToUKTime(this DateTimeOffset dateTimeUtc)
		=> TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTimeUtc.UtcDateTime, "GMT Standard Time");

	/// <summary>
	/// Converts a nullable UTC <see cref="DateTimeOffset"/> to UK local time, or null if null.
	/// </summary>
	public static DateTime? ToUKTime(this DateTimeOffset? dateTimeUtc)
		=> dateTimeUtc switch
		{
			not null => TimeZoneInfo.ConvertTimeBySystemTimeZoneId(((DateTimeOffset)dateTimeUtc).UtcDateTime, "GMT Standard Time"),
			null => null
		};

	/// <summary>
	/// Converts a <see cref="DateTimeOffset"/> to a UTC string representation.
	/// </summary>
	public static string ToUtcString(this DateTimeOffset dateTimeUtc)
		=> dateTimeUtc.ToString("u").Replace(" ", "T");

	/// <summary>
	/// Converts a nullable <see cref="DateTimeOffset"/> to a UTC string representation, or null if null.
	/// </summary>
	public static string? ToUtcString(this DateTimeOffset? dateTimeUtc)
		=> dateTimeUtc switch
		{
			not null => ((DateTimeOffset)dateTimeUtc).ToUtcString(),
			null => null
		};

}
