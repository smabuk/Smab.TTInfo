namespace Smab.Shared.Helpers;

public static class DateOnlyExtensions
{
	internal static readonly string DD_MMMM_YYYY = "dd MMMM, yyyy";

	public static string ToDateLongMonthYearString(this DateOnly date) => date.ToString(DD_MMMM_YYYY);
	public static string ToDateLongMonthYearString(this DateOnly? date) => date?.ToString(DD_MMMM_YYYY) ?? "";

	public static string ToUtcString(this DateOnly date) =>date.ToDateTime(TimeOnly.MinValue).ToUtcString();
	public static string? ToUtcString(this DateOnly? date)
		=> date switch
		{
			not null => ((DateOnly)date).ToUtcString(),
			null => null
		};

}
