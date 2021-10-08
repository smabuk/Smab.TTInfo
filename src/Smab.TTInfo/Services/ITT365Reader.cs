namespace Smab.TTInfo;

public interface ITT365Reader
{
	string League { get; set; }
	string Season { get; set; }
	bool UseTestFiles { get; set; }

	IcalCalendar IcalFromFixtures(string LeagueName, string TeamName, ICollection<Fixture> Fixtures, TimeZoneInfo timeZone);
	string IcalStringFromFixtures(string LeagueName, string TeamName, ICollection<Fixture> Fixtures, TimeZoneInfo timeZone);
	Task<FixturesView?> GetFixturesByTeamName(string TeamName = "");
	Task<FixturesView?> GetFixturesByTeamName(string TeamName = "", string? League = null, string? Season = null);
	Task<Team?> GetTeamStats(string TeamName);

}
