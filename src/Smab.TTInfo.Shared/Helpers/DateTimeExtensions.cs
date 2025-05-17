namespace Smab.Shared.Helpers;

/// <summary>
/// Extension methods for <see cref="DateTime"/> to provide formatted string representations.
/// </summary>
public static class DateTimeExtensions
{
	internal static readonly string DD_MMMM_YYYY = "dd MMMM, yyyy";

	/// <summary>
	/// Converts a <see cref="DateTime"/> to a string in the format "dd MMMM, yyyy".
	/// </summary>
	public static string ToDateLongMonthYearString(this DateTime dateTime) => dateTime.ToString(DD_MMMM_YYYY);

	/// <summary>
	/// Converts a nullable <see cref="DateTime"/> to a string in the format "dd MMMM, yyyy" or an empty string if null.
	/// </summary>
	public static string ToDateLongMonthYearString(this DateTime? dateTime) => dateTime?.ToString(DD_MMMM_YYYY) ?? "";

	/// <summary>
	/// Converts a <see cref="DateTime"/> to a UTC string representation.
	/// </summary>
	public static string ToUtcString(this DateTime dateTime)
		=> dateTime.ToUniversalTime().ToString("u").Replace(" ", "T");

	/// <summary>
	/// Converts a nullable <see cref="DateTime"/> to a UTC string representation, or null if null.
	/// </summary>
	public static string? ToUtcString(this DateTime? dateTime)
		=> dateTime switch
		{
			not null => ((DateTime)dateTime).ToUtcString(),
			null => null
		};

}
