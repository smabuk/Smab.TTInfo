using Smab.Calendar;

namespace Smab.TTInfo.Server.EndPoints;
public static partial class CalendarEndPoints
{
	public const string DefaultCalendarByTeamRoute = "/calendar/{LeagueName}/{TeamName}";

	public static void MapCalendarEndPoints(this WebApplication? app)
	{
		app?.MapGet(DefaultCalendarByTeamRoute, GetCalendarByTeam);
	}

	public static Func<string, string, string?, ITT365Reader, HttpContext, Task<IResult>> GetCalendarByTeam =
		async (string LeagueName, string TeamName, string? Command, ITT365Reader _tt365, HttpContext context) =>
		{
			TeamName = TeamName.Replace("_", " ");
			List<Fixture>? fixtures = (await _tt365.GetAllFixtures(LeagueName))?
						.Where(f => string.Equals(f.HomeTeam, TeamName, StringComparison.CurrentCultureIgnoreCase) || string.Equals(f.AwayTeam, TeamName, StringComparison.CurrentCultureIgnoreCase))
                        .ToList();
			if (fixtures is null)
			{
				return Results.NotFound();
			}

			TimeZoneInfo gmtZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

			IcalCalendar ical = _tt365.IcalFromFixtures(LeagueName, TeamName, fixtures, gmtZone);

			// Different ways of returning the information
			switch (Command?.ToUpper())
			{
				case "TEXT":
					return Results.Content(ical.ToString(), "text/plain");
				case "CONTENT":
					context.Response.Headers.Add("content-disposition", $"inline;filename={LeagueName} - {TeamName} Fixtures.ics");
					return Results.Content(ical.ToString(), "text/calendar", System.Text.Encoding.UTF8);
				case "FILE":
					return Results.File(System.Text.Encoding.UTF8.GetBytes(ical.ToString()), "text/calendar", $"{LeagueName} - {TeamName} Fixtures.ics");
				case "CSV":
					return Results.File(System.Text.Encoding.UTF8.GetBytes(_tt365.CsvFromFixtures(fixtures)), "text/plain", $"{LeagueName} - {TeamName} Fixtures.csv");
				case "JSON":
					return Results.Json(ical);
				case "NEG":
					return Results.Content(ical.ToString());
				default:
					break;
			}

			return Results.File(System.Text.Encoding.UTF8.GetBytes(ical.ToString()), "text/calendar", $"{LeagueName} - {TeamName} Fixtures.ics");

		};

}
