namespace Smab.TTInfo;

public interface ITT365Reader
{
	bool UseTestFiles { get; set; }

	Task<List<Fixture>?> GetAllFixtures(string LeagueId, string? SeasonId = null);

	//Task<FixturesView?> GetFixturesByTeamName(string TeamName);
	//Task<FixturesView?> GetFixturesByTeamName(string LeagueId, string Seasonid, string TeamName);
	
	Task<League?> GetLeague(string LeagueId);
	Task<Team?> GetTeamStats(string LeagueId, string TeamName);

	IcalCalendar IcalFromFixtures(string LeagueId, string TeamName, ICollection<Fixture> Fixtures, TimeZoneInfo timeZone);
	string IcalStringFromFixtures(string LeagueId, string TeamName, ICollection<Fixture> Fixtures, TimeZoneInfo timeZone);

}
