namespace Smab.Shared.Helpers;

public static class DateTimeExtensions
{
	internal static readonly string DD_MMMM_YYYY = "dd MMMM, yyyy";

	public static string ToDateLongMonthYearString(this DateTime dateTime) => dateTime.ToString(DD_MMMM_YYYY);
	public static string ToDateLongMonthYearString(this DateTime? dateTime) => dateTime?.ToString(DD_MMMM_YYYY) ?? "";

	public static string ToUtcString(this DateTime dateTime)
	=> dateTime.ToUniversalTime().ToString("u").Replace(" ", "T");
	public static string? ToUtcString(this DateTime? dateTime)
		=> dateTime switch
		{
			not null => ((DateTime)dateTime).ToUtcString(),
			null => null
		};

}
