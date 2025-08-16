using Smab.Calendar;
using Smab.TTInfo.TT365.Models.TT365;
using Smab.TTInfo.TTLeagues.Models.TTLeagues;

using static Smab.TTInfo.Shared.LeagueRouter;

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
	/// Retrieves the calendar of fixtures for a specified team in a given league and returns it in the requested format.
	/// </summary>
	/// <remarks>This method determines the league type based on the <paramref name="LeagueName"/> and retrieves the
	/// fixtures for the specified <paramref name="TeamName"/>. The fixtures are then converted into an iCalendar format
	/// and returned in the format specified by <paramref name="Command"/>. If no fixtures are found, a "Not Found" result
	/// is returned.</remarks>
	/// <param name="LeagueName">The name of the league to which the team belongs. This value determines the league type.</param>
	/// <param name="TeamName">The name of the team whose fixtures are to be retrieved. Underscores in the name will be replaced with spaces.</param>
	/// <param name="Command">The format in which the calendar should be returned. Supported values are: <list type="bullet">
	/// <item><description><c>TEXT</c>: Returns the calendar as plain text.</description></item>
	/// <item><description><c>CONTENT</c>: Returns the calendar as an inline calendar file with appropriate
	/// headers.</description></item> <item><description><c>FILE</c>: Returns the calendar as a downloadable calendar
	/// file.</description></item> <item><description><c>JSON</c>: Returns the calendar in JSON
	/// format.</description></item> <item><description><c>NEG</c>: Returns the calendar as plain text (default
	/// behavior).</description></item> </list> If <paramref name="Command"/> is null or unrecognized, the calendar will be
	/// returned as a downloadable calendar file.</param>
	/// <param name="_tt365">An instance of <see cref="ITT365Reader"/> used to retrieve fixtures for TT365 leagues.</param>
	/// <param name="_ttleagues">An instance of <see cref="TTLeaguesReader"/> used to retrieve fixtures for TTLeagues leagues.</param>
	/// <param name="context">The current HTTP context, used to set response headers when necessary.</param>
	/// <returns>A <see cref="Results{T1, T2, T3, T4}"/> object containing one of the following results: <list type="bullet">
	/// <item><description><see cref="ContentHttpResult"/>: The calendar as plain text or inline
	/// content.</description></item> <item><description><see cref="FileContentHttpResult"/>: The calendar as a
	/// downloadable file.</description></item> <item><description><see cref="JsonHttpResult{T}"/>: The calendar in JSON
	/// format.</description></item> <item><description><see cref="NotFound"/>: Indicates that no fixtures were found for
	/// the specified team.</description></item> </list></returns>
	public static async Task<Results<ContentHttpResult, FileContentHttpResult, JsonHttpResult<IcalCalendar>, NotFound>> GetCalendarByTeam(string LeagueName, string TeamName, string? Command, ITT365Reader _tt365, TTLeaguesReader _ttleagues, HttpContext context)
	{
		TimeZoneInfo gmtZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
		LeagueType leagueType = GetLeagueType(LeagueName);
		TeamName = TeamName.Replace("_", " ");
		IcalCalendar? ical = default;

		if (leagueType is LeagueType.TT365) {
			TT365LeagueId leagueId= (TT365LeagueId)LeagueName;
			List<Fixture>? fixtures = (await _tt365.GetAllFixtures(leagueId))?
						.Where(f => string.Equals(f.HomeTeam, TeamName, StringComparison.CurrentCultureIgnoreCase) || string.Equals(f.AwayTeam, TeamName, StringComparison.CurrentCultureIgnoreCase))
						.ToList();
			if (fixtures is null) {
				return TypedResults.NotFound();
			}

			ical = _tt365.IcalFromFixtures(leagueId, TeamName, fixtures, gmtZone);
		} else {
			string ttinfoId = LeagueName;
			Fixtures? allFixtures = await _ttleagues.GetAllFixturesWithMatchResults(ttinfoId);

			List<Match> fixtures = allFixtures?.Matches
				.Where(f => string.Equals(f.Home.Name, TeamName, StringComparison.CurrentCultureIgnoreCase) || string.Equals(f.Away.Name, TeamName, StringComparison.CurrentCultureIgnoreCase))
				.Where(f => !(string.Equals(f.Home.Name, "Free", StringComparison.CurrentCultureIgnoreCase) || string.Equals(f.Away.Name, "Free", StringComparison.CurrentCultureIgnoreCase)))
				.OrderBy(m => m.ActualDateTime)
				.ToList() ?? [];

			if (fixtures is []) {
				return TypedResults.NotFound();
			}

			ical = _ttleagues.IcalFromFixtures(LeagueName, TeamName, fixtures, gmtZone);
		}

		// Different ways of returning the information
		switch (Command?.ToUpperInvariant()) {
			case "TEXT":
				return TypedResults.Content(ical.ToString(), "text/plain");
			case "CONTENT":
				context.Response.Headers.Append("content-disposition", $"inline;filename={LeagueName} - {TeamName} Fixtures.ics");
				return TypedResults.Content(ical.ToString(), "text/calendar", System.Text.Encoding.UTF8);
			case "FILE":
				return TypedResults.File(System.Text.Encoding.UTF8.GetBytes(ical.ToString()), "text/calendar", $"{LeagueName} - {TeamName} Fixtures.ics");
			//case "CSV":
			//	return TypedResults.File(System.Text.Encoding.UTF8.GetBytes(_tt365.CsvFromFixtures(fixtures)), "text/plain", $"{LeagueName} - {TeamName} Fixtures.csv");
			case "JSON":
				return TypedResults.Json(ical);
			case "NEG":
				return TypedResults.Content(ical.ToString());
			default:
				return TypedResults.File(System.Text.Encoding.UTF8.GetBytes(ical.ToString()), "text/calendar", $"{LeagueName} - {TeamName} Fixtures.ics");
		}
	}
}
