namespace Smab.TTInfo;

public interface ITT365Reader
{
	string LeagueId { get; set; }
	string Season { get; set; }
	bool UseTestFiles { get; set; }

	Task<FixturesView?> GetFixturesByTeamName(string TeamName = "");

	Task<FixturesView?> GetFixturesByTeamName(string TeamName = "", string? League = null, string? Season = null);
	Task<League?> GetLeague(string LeagueName);
	Task<Team?> GetTeamStats(string TeamName);

	IcalCalendar IcalFromFixtures(string LeagueName, string TeamName, ICollection<Fixture> Fixtures, TimeZoneInfo timeZone);
	string IcalStringFromFixtures(string LeagueName, string TeamName, ICollection<Fixture> Fixtures, TimeZoneInfo timeZone);

}
