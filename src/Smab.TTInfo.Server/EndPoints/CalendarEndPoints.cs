using Smab.Calendar;

namespace Smab.TTInfo.Server.EndPoints;

/// <summary>
/// Provides endpoints for handling calendar-related operations, such as retrieving fixtures for a specific team.
/// </summary>
/// <remarks>This class defines constants and extension methods for mapping and handling calendar-related HTTP
/// endpoints. It includes functionality to retrieve and format team fixtures in various formats, such as iCalendar,
/// plain text, CSV, or JSON.</remarks>
public static partial class CalendarEndPoints
{
	public const string DefaultCalendarByTeamRoute = "/calendar/{LeagueName}/{TeamName}";

	/// <summary>
	/// Maps the calendar-related endpoints to the specified <see cref="WebApplication"/> instance.
	/// </summary>
	/// <remarks>This method registers the default calendar endpoint for retrieving calendar data by team. Ensure
	/// that the <paramref name="app"/> parameter is properly initialized before calling this method.</remarks>
	/// <param name="app">The <see cref="WebApplication"/> instance to which the endpoints will be mapped. Must not be <see
	/// langword="null"/>.</param>
	public static void MapCalendarEndPoints(this WebApplication? app) 
		=> _ = (app?.MapGet(DefaultCalendarByTeamRoute, GetCalendarByTeam));

	/// <summary>
	/// Retrieves a calendar of fixtures for a specific team in a given league, formatted according to the specified
	/// command.
	/// </summary>
	/// <remarks>The method retrieves all fixtures for the specified team in the given league and generates a
	/// calendar in iCalendar format. The output format is determined by the <paramref name="Command"/> parameter. If no
	/// command is provided, the default behavior is to return the calendar as a downloadable calendar file.</remarks>
	/// <param name="LeagueName">The name of the league to which the team belongs. This value cannot be null or empty.</param>
	/// <param name="TeamName">The name of the team for which the fixtures are being retrieved. Underscores in the name will be replaced with
	/// spaces.</param>
	/// <param name="Command">An optional command specifying the desired output format. Supported values are: <list type="bullet">
	/// <item><description><c>TEXT</c>: Returns the calendar as plain text.</description></item>
	/// <item><description><c>CONTENT</c>: Returns the calendar as an inline calendar file with appropriate
	/// headers.</description></item> <item><description><c>FILE</c>: Returns the calendar as a downloadable calendar
	/// file.</description></item> <item><description><c>CSV</c>: Returns the fixtures as a CSV file.</description></item>
	/// <item><description><c>JSON</c>: Returns the calendar in JSON format.</description></item>
	/// <item><description><c>NEG</c>: Returns the calendar as plain text without additional headers.</description></item>
	/// <item><description>Any other value defaults to returning the calendar as a downloadable calendar
	/// file.</description></item> </list></param>
	/// <param name="_tt365">An instance of <see cref="ITT365Reader"/> used to retrieve fixture and calendar data.</param>
	/// <param name="context">The current HTTP context, used to set response headers when necessary.</param>
	/// <returns>A <see cref="Results{T1, T2, T3, T4}"/> object containing one of the following results: <list type="bullet">
	/// <item><description><see cref="ContentHttpResult"/>: If the calendar is returned as plain text or inline
	/// content.</description></item> <item><description><see cref="FileContentHttpResult"/>: If the calendar or fixtures
	/// are returned as a downloadable file.</description></item> <item><description><see cref="JsonHttpResult{T}"/>: If
	/// the calendar is returned in JSON format.</description></item> <item><description><see cref="NotFound"/>: If no
	/// fixtures are found for the specified team.</description></item> </list></returns>
	public static async Task<Results<ContentHttpResult, FileContentHttpResult, JsonHttpResult<IcalCalendar>, NotFound>> GetCalendarByTeam(string LeagueName, string TeamName, string? Command, ITT365Reader _tt365, HttpContext context)
	{
		TeamName = TeamName.Replace("_", " ");
		List<Fixture>? fixtures = (await _tt365.GetAllFixtures(LeagueName))?
					.Where(f => string.Equals(f.HomeTeam, TeamName, StringComparison.CurrentCultureIgnoreCase) || string.Equals(f.AwayTeam, TeamName, StringComparison.CurrentCultureIgnoreCase))
					.ToList();
		if (fixtures is null) {
			return TypedResults.NotFound();
		}

		TimeZoneInfo gmtZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

		IcalCalendar ical = _tt365.IcalFromFixtures(LeagueName, TeamName, fixtures, gmtZone);

		// Different ways of returning the information
		switch (Command?.ToUpperInvariant()) {
			case "TEXT":
				return TypedResults.Content(ical.ToString(), "text/plain");
			case "CONTENT":
				context.Response.Headers.Append("content-disposition", $"inline;filename={LeagueName} - {TeamName} Fixtures.ics");
				return TypedResults.Content(ical.ToString(), "text/calendar", System.Text.Encoding.UTF8);
			case "FILE":
				return TypedResults.File(System.Text.Encoding.UTF8.GetBytes(ical.ToString()), "text/calendar", $"{LeagueName} - {TeamName} Fixtures.ics");
			case "CSV":
				return TypedResults.File(System.Text.Encoding.UTF8.GetBytes(_tt365.CsvFromFixtures(fixtures)), "text/plain", $"{LeagueName} - {TeamName} Fixtures.csv");
			case "JSON":
				return TypedResults.Json(ical);
			case "NEG":
				return TypedResults.Content(ical.ToString());
			default:
				return TypedResults.File(System.Text.Encoding.UTF8.GetBytes(ical.ToString()), "text/calendar", $"{LeagueName} - {TeamName} Fixtures.ics");
		}
	}
}
