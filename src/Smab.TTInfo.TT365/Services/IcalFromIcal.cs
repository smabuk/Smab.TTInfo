using Ical.Net.CalendarComponents;

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
	public async Task<string> IcalStringFromIcal(TT365LeagueId leagueId, string TeamName, TimeZoneInfo timeZone)
		=> (await IcalFromIcal(leagueId, TeamName, timeZone)).ToString() ?? "";

	public async Task<Ical.Net.Calendar> IcalFromIcal(TT365LeagueId leagueId, string TeamName, TimeZoneInfo timeZone)
	{
		League league = await GetLeague(leagueId) ?? throw new InvalidOperationException($"League with ID {leagueId} not found.");

		string teamId = league.CurrentSeason.Lookups.TeamLookup.FirstOrDefault(t =>	t.Name.Equals(TeamName, StringComparison.OrdinalIgnoreCase))?.Id ?? "";
		string divisionId = league.CurrentSeason.Divisions.SingleOrDefault(d => d.Teams.Any(t => t.Id == teamId))?.Id ?? "";

		TT365LeagueId reading = new("Reading");
		TT365LeagueId bracknell = new("BracknellAndWokingham");

		// TODO: This is a temporary solution to map division names to their corresponding IDs for specific leagues. A more robust solution would involve fetching the correct division ID based on the team name and league from the TT365 API or data source.
		divisionId = divisionId switch
		{
			"Division 1" when leagueId == reading => "9608",
			"Division 2" when leagueId == reading => "9609",
			"Division 3" when leagueId == reading => "9610",
			"Division 4" when leagueId == reading => "9611",
			"Premier" when leagueId == bracknell => "9692",
			"Division 1" when leagueId == bracknell => "9693",
			"Division 2" when leagueId == bracknell => "9694",
			_ => throw new NotImplementedException($"Need to figure out how to get the division ID from the team name '{TeamName}' in league '{leagueId}'."),
		};

		if (string.IsNullOrWhiteSpace(divisionId)) {
			throw new NotImplementedException($"IcalFromIcal is not implemented yet. Need to figure out how to get the division ID from the team name in league {leagueId}.");
		}

		string url = $"Fixtures/calendar.ics?d={divisionId}&t={teamId}";
		Ical.Net.Calendar? ical = await LoadAsync<Ical.Net.Calendar>(
			leagueId,
			url
		) ?? throw new InvalidOperationException($"Failed to load iCalendar data for league '{leagueId}', team '{TeamName}', division '{divisionId}'.");

		// Fix the times from All day to start at a default of 7:30pm UTC time, as TT365 does not provide times for fixtures in the iCal feed
		foreach (CalendarEvent e in ical.Events) {
			if (e.IsAllDay) {
				TimeOnly defaultTime = DEFAULT_START_TIME; // 7:30pm

				TimeOnly startTime=  e.Location switch
				{
					string v when v.Contains("RBL", StringComparison.OrdinalIgnoreCase) => defaultTime.AddMinutes(-30), // 7:00pm start time
					string v when v.Contains("CURZON", StringComparison.OrdinalIgnoreCase) => defaultTime.AddMinutes(-30), // 7:00pm start time
					string v when v.Contains("BRAYBROOKE", StringComparison.OrdinalIgnoreCase) => defaultTime.AddMinutes(-15), // 7:15pm start time
					_ => defaultTime
				};

				Ical.Net.DataTypes.CalDateTime startDateTime = new(e.Start!.Value.Add(startTime.ToTimeSpan()));
				e.Start = startDateTime;
				e.End = new(startDateTime.Value.AddHours(3));
			}
		}

		return ical ?? new();
	}
}
