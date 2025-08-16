using Smab.Calendar;

namespace Smab.TTInfo.TTLeagues.Services;
public sealed partial class TTLeaguesReader
{
	public string IcalStringFromFixtures(string ttinfoId, string TeamName, ICollection<Match> Fixtures, TimeZoneInfo timeZone)
		=> IcalFromFixtures(ttinfoId, TeamName, Fixtures, timeZone).ToString();

	public IcalCalendar IcalFromFixtures(string ttinfoId, string TeamName, ICollection<Match> Fixtures, TimeZoneInfo timeZone)
	{
		IcalCalendar ical = new()
		{
			Name = $"{ttinfoId} fixtures - {TeamName}",
			Description = $"Fixtures and results of matches for the {ttinfoId} league",
			Events = []
		};

		foreach (Match match in Fixtures)
		{
			string homeTeam = match.Home.DisplayName;
			string awayTeam = match.Away.DisplayName;
			string venue = match.Venue;
			DateTimeOffset? actualDateTime = match.ActualDateTime;

			DateTime dateStart = (actualDateTime is not null
				? TimeZoneInfo.ConvertTimeToUtc(actualDateTime.Value.Date + new TimeSpan(19, 30, 0), timeZone)
				: TimeZoneInfo.ConvertTimeToUtc((match.Date?.Date ?? DateTimeOffset.Now.Date) + new TimeSpan(19, 30, 0), timeZone));
			DateTime dateEnd = (actualDateTime is not null
				? TimeZoneInfo.ConvertTimeToUtc(actualDateTime.Value.Date + new TimeSpan(22, 30, 0), timeZone)
				: TimeZoneInfo.ConvertTimeToUtc((match.Date?.Date ?? DateTimeOffset.Now.Date) + new TimeSpan(22, 30, 0), timeZone));

			VEvent fixtureEvent = new()
			{
				UID = $"{ttinfoId} {homeTeam} vs {awayTeam}",
				Summary = $"🏓 {homeTeam} vs {awayTeam}",
				Location = venue,
				DateStart = dateStart,
				DateEnd = dateEnd,
				Priority = VEvent.PriorityLevel.Normal,
				Transparency = VEvent.TransparencyType.TRANSPARENT,
				Categories = "Table tennis,OLOP Table Tennis Club",
				Description = "\n"
			};

			if (venue is not null && (venue.Contains("CURZON", StringComparison.OrdinalIgnoreCase) || venue.Contains("RBL", StringComparison.OrdinalIgnoreCase)))
			{
				fixtureEvent.DateStart = fixtureEvent.DateStart.AddMinutes(-30);
			}

			if (!string.IsNullOrEmpty(TeamName))
			{
				fixtureEvent.Transparency = VEvent.TransparencyType.OPAQUE;
				fixtureEvent.Alarms =
				[
					new VAlarm
					{
						Trigger = new System.TimeSpan(0, 0, 60, 0),
						Action = VAlarm.ActionType.DISPLAY,
						Description = "Reminder"
					}
				];
			}

			if (match.HasResults)
			{
				int homeScore = match.HomeScores?.Sum(x => x.Score) ?? 0;
				int awayScore = match.AwayScores?.Sum(x => x.Score) ?? 0;
				if (homeScore > awayScore)
				{
					fixtureEvent.Description += $"\nWIN:  {homeTeam.ToUpper()}";
					fixtureEvent.Description += $"\nLOSS: {awayTeam}";
				}
				else if (homeScore < awayScore)
				{
					fixtureEvent.Description += $"\nLOSS: {homeTeam}";
					fixtureEvent.Description += $"\nWIN:  {awayTeam.ToUpper()}";
				}
				else if (homeScore == awayScore)
				{
					fixtureEvent.Description += $"\nDRAW: {homeTeam} and {awayTeam}";
				}
				fixtureEvent.Description += $"\nScore: {homeScore}-{awayScore}";
			}

			ical.Events.Add(fixtureEvent);
		}

		return ical;
	}
}
