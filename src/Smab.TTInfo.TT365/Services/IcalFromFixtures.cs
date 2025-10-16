using System.Text.RegularExpressions;

namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
	/// <summary>
	/// Generates an iCalendar (iCal) string representation of the specified fixtures for a given league and team.
	/// </summary>
	/// <param name="leagueId">The Id of the league to which the fixtures belong. Cannot be null or empty.</param>
	/// <param name="TeamName">The name of the team for which the fixtures are being generated. Cannot be null or empty.</param>
	/// <param name="Fixtures">A collection of fixtures to include in the iCalendar. Cannot be null or empty.</param>
	/// <param name="timeZone">The time zone to use for the fixture dates and times. Cannot be null.</param>
	/// <returns>A string containing the iCalendar representation of the fixtures.</returns>
	public string IcalStringFromFixtures(TT365LeagueId leagueId, string TeamName, ICollection<Fixture> Fixtures, TimeZoneInfo timeZone)
		=> IcalFromFixtures(leagueId, TeamName, Fixtures, timeZone).ToString();

	public IcalCalendar IcalFromFixtures(TT365LeagueId leagueId, string TeamName, ICollection<Fixture> Fixtures, TimeZoneInfo timeZone)
	{
		IcalCalendar ical = new()
		{
			Name = $"{leagueId} fixtures - {TeamName}",
			Description = $"Fixtures and results of matches for the {leagueId} league",
			Events = []
		};

		foreach (Fixture fixture in Fixtures) {
			VEvent fixtureEvent = new()
			{
				UID          = $"TT365 {fixture.Date:yyyyMMdd} {fixture.HomeTeam} vs {fixture.AwayTeam}",
				Summary      = $"🏓 {fixture.HomeTeam} vs {fixture.AwayTeam}",
				Location     = fixture.Venue,
				DateStart    = TimeZoneInfo.ConvertTimeToUtc(fixture.Date.ToDateTime(new(19, 30)), timeZone), // All matches by default start at 7:30pm
				DateEnd      = TimeZoneInfo.ConvertTimeToUtc(fixture.Date.ToDateTime(new(22, 30)), timeZone),
				Priority     = VEvent.PriorityLevel.Normal,
				Transparency = VEvent.TransparencyType.TRANSPARENT,
				Categories   = "Table tennis,OLOP Table Tennis Club",
				Description  = $"\n"
			};

			if (fixture.Venue.Contains("CURZON", StringComparison.OrdinalIgnoreCase)
			|| fixture.Venue.Contains("RBL", StringComparison.OrdinalIgnoreCase)) // 7pm start time
			{
				fixtureEvent.DateStart = fixtureEvent.DateStart.AddMinutes(-30);
			}

			if (fixture.Venue.Contains("BRAYBROOKE", StringComparison.OrdinalIgnoreCase)) // 7:15pm start time
			{
				fixtureEvent.DateStart = fixtureEvent.DateStart.AddMinutes(-15);
			}

			if (!string.IsNullOrEmpty(TeamName)) // If looking at a particular team add BUSY and 1hr REMINDER
			{
				fixtureEvent.Transparency = VEvent.TransparencyType.OPAQUE;
				fixtureEvent.Alarms =
					[
						new() {
							Trigger = new System.TimeSpan(0, 0, 60, 0),
							Action = VAlarm.ActionType.DISPLAY,
							Description = "Reminder"
						}
					];
			}

			if (fixture is PostponedFixture postponedFixture) {
				fixtureEvent.Summary = $"🏓 POSTPONED: {postponedFixture.HomeTeam} vs {postponedFixture.AwayTeam}";
				fixtureEvent.Transparency = VEvent.TransparencyType.TRANSPARENT;
				fixtureEvent.Alarms = [];
				fixtureEvent.Description += $"\n{postponedFixture.Reason}\n";
			}

			if (fixture is RearrangedFixture rearrangedFixture) {
				fixtureEvent.Summary += $" (rearranged from {rearrangedFixture.OriginalDate:yyyy-MM-dd})";
				fixtureEvent.Description += $"\n{rearrangedFixture.Reason}\n";
			}

			if (fixture is CompletedFixture completedFixture) {
				if (completedFixture.ForHome > completedFixture.ForAway) {
					fixtureEvent.Description += $"\nWIN:  {fixture.HomeTeam.ToUpper()}";
					fixtureEvent.Description += $"\nLOSS: {fixture.AwayTeam}";
				} else if (completedFixture.ForHome < completedFixture.ForAway) {
					fixtureEvent.Description += $"\nLOSS: {fixture.HomeTeam}";
					fixtureEvent.Description += $"\nWIN:  {fixture.AwayTeam.ToUpper()}";
				} else if (completedFixture.ForHome == completedFixture.ForAway) {
					fixtureEvent.Description += $"\nDRAW: {fixture.HomeTeam} and {fixture.AwayTeam}";
				};
				fixtureEvent.Description += $"\nScore: {completedFixture.Score}";
			}

			ical.Events.Add(fixtureEvent);
		}

		return ical;
	}
}
