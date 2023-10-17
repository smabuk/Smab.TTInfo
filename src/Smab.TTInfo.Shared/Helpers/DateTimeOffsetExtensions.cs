namespace Smab.Shared.Helpers;

public static class DateTimeOffsetExtensions
{
	internal static readonly string DD_MMMM_YYYY = "dd MMMM, yyyy";

	public static DateTime ToUKTime(this DateTimeOffset dateTimeUtc)
		=> TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTimeUtc.UtcDateTime, "GMT Standard Time");
	public static DateTime? ToUKTime(this DateTimeOffset? dateTimeUtc)
		=> dateTimeUtc switch
		{
			not null => TimeZoneInfo.ConvertTimeBySystemTimeZoneId(((DateTimeOffset)dateTimeUtc).UtcDateTime, "GMT Standard Time"),
			null => null
		};
	public static string ToUtcString(this DateTimeOffset dateTimeUtc)
		=> dateTimeUtc.ToString("u").Replace(" ", "T");
	public static string? ToUtcString(this DateTimeOffset? dateTimeUtc)
		=> dateTimeUtc switch
		{
			not null => ((DateTimeOffset)dateTimeUtc).ToUtcString(),
			null => null
		};

}
