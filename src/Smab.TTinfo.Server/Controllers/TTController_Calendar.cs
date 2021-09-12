using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Text;
using Smab.Calendar;


namespace Smab.TTInfo.Server.Controllers;
public partial class TTController : Controller
{

    [HttpGet]
	[Route("{League}/{TeamName}")]
	public async Task<IActionResult> Calendar(string? League = null, string TeamName = "", string Command = "")
    {
		TeamName = TeamName.Replace("_", " ");
		FixturesView? tt365FixtureView = await _tt365.GetFixturesByTeamName(TeamName);
		if (tt365FixtureView is null || tt365FixtureView.Fixtures is null) {
			return new EmptyResult();
		}

        TimeZoneInfo gmtZone;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            gmtZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
        }
        else
        {
            gmtZone = TimeZoneInfo.FindSystemTimeZoneById("GMT");
        }

        IcalCalendar ical = new IcalCalendar
        {
            Name = $"RDTTA fixtures - {TeamName}",
            Description = "Fixtures and results of matches for the Reading and District TT Association"
        };

        ical.Events = new List<VEvent>();
        foreach (Fixture fixture in tt365FixtureView.Fixtures)
        {
            VEvent fixtureEvent = new VEvent
            {
                UID = $"RDTTA {fixture.HomeTeam} vs {fixture.AwayTeam}",
                Summary = $"🏓 {fixture.HomeTeam} vs {fixture.AwayTeam}",
                Location = fixture.Venue,
                DateStart = TimeZoneInfo.ConvertTimeToUtc(fixture.Date, gmtZone).AddHours(19).AddMinutes(30), // All matches by default start at 7:30pm
                DateEnd = TimeZoneInfo.ConvertTimeToUtc(fixture.Date, gmtZone).AddHours(22).AddMinutes(30),
                Priority = VEvent.PriorityLevel.Normal,
                Transparency = VEvent.TransparencyType.TRANSPARENT,
                Categories = "Table tennis,OLOP Table Tennis Club",
                Description = $"\n"
            };

            if (fixture.Venue.ToUpperInvariant().Contains("CURZON")
				|| fixture.Venue.ToUpperInvariant().Contains("RBL")) // 7pm start time
            {
                fixtureEvent.DateStart = fixtureEvent.DateStart.AddMinutes(-30);
            }

            if (!string.IsNullOrEmpty(TeamName)) // If looking at a particular team add BUSY and 1hr REMINDER
            {
                fixtureEvent.Transparency = VEvent.TransparencyType.OPAQUE;
                fixtureEvent.Alarms = new List<VAlarm>
                {
                    new VAlarm
                    {
                        Trigger = new System.TimeSpan(0, 0, 60, 0),
                        Action = VAlarm.ActionType.DISPLAY,
                        Description = "Reminder"
                    }
                };
            }

            if (fixture.IsCompleted)
            {
                if (fixture.ForHome > fixture.ForAway)
                {
                    fixtureEvent.Description += $"\nWIN:  {fixture.HomeTeam.ToUpper()}";
                    fixtureEvent.Description += $"\nLOSS: {fixture.AwayTeam}";
                }
                else if (fixture.ForHome < fixture.ForAway)
                {
                    fixtureEvent.Description += $"\nLOSS: {fixture.HomeTeam}";
                    fixtureEvent.Description += $"\nWIN:  {fixture.AwayTeam.ToUpper()}";
                }
                else if (fixture.ForHome == fixture.ForAway)
                {
                    fixtureEvent.Description += $"\nDRAW: {fixture.HomeTeam} and {fixture.AwayTeam}";
                }
                fixtureEvent.Description += $"\nScore: {fixture.Score}";
            }

            ical.Events.Add(fixtureEvent);
        }

        // Different ways of returning the information
        switch (Command?.ToUpper())
        {
            //case "VIEW":
            //    ViewData["Message"] = ical.ToString();
            //    return View(model);
            case "TEXT":
                return Content(ical.ToString(), "text/plain");
            case "CONTENT":
                Response.Headers.Add("content-disposition", "inline;filename=RDTTAFixtures.ics");
                return Content(ical.ToString(), "text/calendar", Encoding.UTF8);
            case "FILE":
                return File(Encoding.UTF8.GetBytes(ical.ToString()), "text/calendar", "RDTTAFixtures.ics");
            case "JSON":
                return Json(ical);
            case "NEG":
                return Content(ical.ToString());
            default:
                break;
        }

        // ToASCIIBytes is an extension method created to encapsulate Encoding.ASCII.GetBytes()
        return File(System.Text.Encoding.UTF8.GetBytes(ical.ToString()), "text/calendar", "RDTTAFixtures.ics");

    }

}
