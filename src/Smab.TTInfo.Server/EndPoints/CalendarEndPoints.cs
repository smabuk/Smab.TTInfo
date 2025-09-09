using Smab.Calendar;

using Smab.TTInfo.TT365.Interfaces;
using Smab.TTInfo.TT365.Models;
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
	/// <remarks>If no fixtures are found for the specified team and league, a NotFound result is returned. The
	/// output format is determined by the Command parameter; for example, "CSV" returns a CSV file, while "JSON" returns
	/// the calendar as JSON. The method supports both TT365 and TTLeagues data sources based on the league name.</remarks>
	/// <param name="LeagueName">The name of the league containing the team. This determines which data source is used to retrieve fixtures.</param>
	/// <param name="TeamName">The name of the team for which to retrieve the calendar. Underscores in the name are treated as spaces.</param>
	/// <param name="Command">An optional command specifying the desired output format. Supported values include "TEXT", "CONTENT", "FILE",
	/// "CSV", "JSON", and "NEG". If null or unrecognized, the default is a downloadable iCalendar file.</param>
	/// <param name="venue">An optional venue name to filter fixtures. If specified, only fixtures at this venue are included.</param>
	/// <param name="_tt365">An implementation of ITT365Reader used to access TT365 league fixture data.</param>
	/// <param name="_ttleagues">An implementation of TTLeaguesReader used to access TTLeagues fixture data.</param>
	/// <param name="context">The current HTTP context, used to set response headers and content type.</param>
	/// <returns>A result containing the team's fixture calendar in the requested format. Returns a content result, file result,
	/// JSON result, or not found result if no fixtures are available.</returns>
	public static async Task<Results<ContentHttpResult, FileContentHttpResult, JsonHttpResult<IcalCalendar>, NotFound>> GetCalendarByTeam(string LeagueName, string TeamName, string? Command, string? venue, ITT365Reader _tt365, TTLeaguesReader _ttleagues, HttpContext context)
	{
		TimeZoneInfo gmtZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
		LeagueType leagueType = GetLeagueType(LeagueName);
		TeamName = TeamName.Replace("_", " ");
		IcalCalendar? ical = default;

		if (leagueType is LeagueType.TT365) {
			bool  teamFilter(Fixture f) => string.Equals(f.HomeTeam, TeamName, StringComparison.CurrentCultureIgnoreCase) || string.Equals(f.AwayTeam, TeamName, StringComparison.CurrentCultureIgnoreCase);
			bool venueFilter(Fixture f) => string.Equals(f.Venue, venue, StringComparison.CurrentCultureIgnoreCase);

			Func<Fixture, bool> filterFixtures = venue is null
				? teamFilter
				: venueFilter;

			TT365LeagueId leagueId = (TT365LeagueId)LeagueName;
			List<Fixture> fixtures = [.. (await _tt365.GetAllFixtures(leagueId))?.Where(filterFixtures) ?? []];
			if (fixtures is []) {
				return TypedResults.NotFound();
			}

			ical = _tt365.IcalFromFixtures(leagueId, TeamName, fixtures, gmtZone);
		} else {
			string ttinfoId = LeagueName;
			Fixtures? allFixtures = await _ttleagues.GetAllFixturesWithMatchResults(ttinfoId);

			List<Match> fixtures = allFixtures?.Matches
				.Where(f => f.ActualDateTime is not null)
				.Where(f => string.Equals(f.Home.Name, TeamName, StringComparison.CurrentCultureIgnoreCase) || string.Equals(f.Away.Name, TeamName, StringComparison.CurrentCultureIgnoreCase))
				.Where(f => !(string.Equals(f.Home.Name, "Free", StringComparison.CurrentCultureIgnoreCase) || string.Equals(f.Away.Name, "Free", StringComparison.CurrentCultureIgnoreCase)))
				.OrderBy(m => m.ActualDateTime)
				.ToList() ?? [];

			if (fixtures is []) {
				return TypedResults.NotFound();
			}

			ical = _ttleagues.IcalFromFixtures(LeagueName, TeamName, fixtures, gmtZone);
		}


		_ = Enum.TryParse<CommandType>(Command, ignoreCase: true, out CommandType commandType);

		// Different ways of returning the information
		return commandType switch
		{
			CommandType.TEXT    => TypedResults.Content(ical.ToString(), "text/plain"),
			CommandType.CONTENT => GenerateWithFilename(),
			CommandType.FILE    => TypedResults.File(System.Text.Encoding.UTF8.GetBytes(ical.ToString()), "text/calendar", $"{LeagueName} - {TeamName} Fixtures.ics"),
			CommandType.CSV     => TypedResults.File(System.Text.Encoding.UTF8.GetBytes(CsvFromCalendar(ical)), "text/plain", $"{LeagueName} - {TeamName} Fixtures.csv"),
			CommandType.JSON    => TypedResults.Json(ical),
			CommandType.NEG     => TypedResults.Content(ical.ToString()), // Negotiated content type, default to text/plain
			_ => TypedResults.File(System.Text.Encoding.UTF8.GetBytes(ical.ToString()), "text/calendar", $"{LeagueName} - {TeamName} Fixtures.ics"),
		};

		ContentHttpResult GenerateWithFilename()
		{
			context.Response.Headers.Append("content-disposition", $"inline; filename={LeagueName} - {TeamName} Fixtures.ics");
			context.Response.ContentType = "text/calendar";
			context.Response.ContentLength = ical.ToString().Length;
			return TypedResults.Content(ical.ToString(), "text/calendar", System.Text.Encoding.UTF8);
		}
	}

	private static string CsvFromCalendar(IcalCalendar ical)
	{
		System.Text.StringBuilder output = new();

		foreach (VEvent calEvent in ical.Events) {
			string homeTeam = calEvent.Summary.Split("vs")[0].Replace("🏓", "").Trim();
			string awayTeam = calEvent.Summary.Split("vs")[1].Trim();
			_ = output.Append($"{calEvent.DateStart:dd/MM/yyyy},{homeTeam},{awayTeam},{calEvent.Location}{Environment.NewLine}");
		}

		return output.ToString();
	}

	private enum CommandType {
		UNKNOWN,
		TEXT,
		CONTENT,
		FILE,
		JSON,
		NEG,
		CSV
	}

}
