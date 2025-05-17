namespace Smab.Shared.Helpers;

/// <summary>
/// Extension methods for <see cref="DateOnly"/> to provide formatted string representations.
/// </summary>
public static class DateOnlyExtensions
{
	internal static readonly string DD_MMMM_YYYY = "dd MMMM, yyyy";

	/// <summary>
	/// Converts a <see cref="DateOnly"/> to a string in the format "dd MMMM, yyyy".
	/// </summary>
	public static string ToDateLongMonthYearString(this DateOnly date) => date.ToString(DD_MMMM_YYYY);

	/// <summary>
	/// Converts a nullable <see cref="DateOnly"/> to a string in the format "dd MMMM, yyyy" or an empty string if null.
	/// </summary>
	public static string ToDateLongMonthYearString(this DateOnly? date) => date?.ToString(DD_MMMM_YYYY) ?? "";

	/// <summary>
	/// Converts a <see cref="DateOnly"/> to a UTC string representation.
	/// </summary>
	public static string ToUtcString(this DateOnly date) =>date.ToDateTime(TimeOnly.MinValue).ToUtcString();

	/// <summary>
	/// Converts a nullable <see cref="DateOnly"/> to a UTC string representation, or null if null.
	/// </summary>
	public static string? ToUtcString(this DateOnly? date)
		=> date switch
		{
			not null => ((DateOnly)date).ToUtcString(),
			null => null
		};

}
