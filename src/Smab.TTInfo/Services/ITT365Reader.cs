namespace Smab.TTInfo;

public interface ITT365Reader
{
	string LeagueId { get; set; }
	string SeasonId { get; set; }
	bool UseTestFiles { get; set; }

	Task<List<Fixture>?> GetAllFixtures(string LeagueId, string? SeasonId = null);

	Task<FixturesView?> GetFixturesByTeamName(string TeamName);
	Task<FixturesView?> GetFixturesByTeamName(string TeamName = "", string? LeagueId = null, string? Seasonid = null);
	
	Task<League?> GetLeague(string LeagueId);
	Task<Team?> GetTeamStats(string LeagueId, string TeamName);

	IcalCalendar IcalFromFixtures(string LeagueId, string TeamName, ICollection<Fixture> Fixtures, TimeZoneInfo timeZone);
	string IcalStringFromFixtures(string LeagueId, string TeamName, ICollection<Fixture> Fixtures, TimeZoneInfo timeZone);

}
