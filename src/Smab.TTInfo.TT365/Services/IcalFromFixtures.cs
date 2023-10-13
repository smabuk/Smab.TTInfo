namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
	public string IcalStringFromFixtures(string LeagueName, string TeamName, ICollection<Fixture> Fixtures, TimeZoneInfo timeZone)
		=> IcalFromFixtures(LeagueName, TeamName, Fixtures, timeZone).ToString();

	public IcalCalendar IcalFromFixtures(string LeagueName, string TeamName, ICollection<Fixture> Fixtures, TimeZoneInfo timeZone)
	{
		IcalCalendar ical = new()
		{
			Name = $"{LeagueName} fixtures - {TeamName}",
			Description = $"Fixtures and results of matches for the {LeagueName} league",
			Events = []
		};

		foreach (Fixture fixture in Fixtures) {
			VEvent fixtureEvent = new()
			{
				UID          = $"TT365 {fixture.HomeTeam} vs {fixture.AwayTeam}",
				Summary      = $"🏓 {fixture.HomeTeam} vs {fixture.AwayTeam}",
				Location     = fixture.Venue,
				DateStart    = TimeZoneInfo.ConvertTimeToUtc(fixture.Date.ToDateTime(new(19, 30)), timeZone), // All matches by default start at 7:30pm
				DateEnd      = TimeZoneInfo.ConvertTimeToUtc(fixture.Date.ToDateTime(new(22, 30)), timeZone),
				Priority     = VEvent.PriorityLevel.Normal,
				Transparency = VEvent.TransparencyType.TRANSPARENT,
				Categories   = "Table tennis,OLOP Table Tennis Club",
				Description  = $"\n"
			};

			if (fixture.Venue.Contains("CURZON", StringComparison.InvariantCultureIgnoreCase)
			|| fixture.Venue.Contains("RBL", StringComparison.InvariantCultureIgnoreCase)) // 7pm start time
			{
				fixtureEvent.DateStart = fixtureEvent.DateStart.AddMinutes(-30);
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

			if (fixture is CompletedFixture completedFixture) {
				if (completedFixture.ForHome > completedFixture.ForAway) {
					fixtureEvent.Description += $"\nWIN:  {fixture.HomeTeam.ToUpper()}";
					fixtureEvent.Description += $"\nLOSS: {fixture.AwayTeam}";
				} else if (completedFixture.ForHome < completedFixture.ForAway) {
					fixtureEvent.Description += $"\nLOSS: {fixture.HomeTeam}";
					fixtureEvent.Description += $"\nWIN:  {fixture.AwayTeam.ToUpper()}";
				} else if (completedFixture.ForHome == completedFixture.ForAway) {
					fixtureEvent.Description += $"\nDRAW: {fixture.HomeTeam} and {fixture.AwayTeam}";
				}
				fixtureEvent.Description += $"\nScore: {completedFixture.Score}";
			}

			ical.Events.Add(fixtureEvent);
		}

		return ical;
	}
}
