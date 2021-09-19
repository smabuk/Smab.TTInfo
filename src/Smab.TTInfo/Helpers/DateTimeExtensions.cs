namespace Smab.Shared.Helpers;

public static class DateTimeExtensions
{
    internal static readonly string DD_MMMM_YYYY = "dd MMMM, yyyy";

	public static string ToDateLongMonthYearString(this DateTime dateTime) => dateTime.ToString(DD_MMMM_YYYY);
}
