namespace Smab.TTInfo;

public interface ITT365Reader
{
	bool UseTestFiles { get; set; }

	//Task<FixturesView?> GetFixturesByTeamName(string TeamName);
	//Task<FixturesView?> GetFixturesByTeamName(string LeagueId, string Seasonid, string TeamName);

	Task<List<Fixture>?>  GetAllFixtures(string LeagueId, string? SeasonId = null);

	Task<List<Division>>  GetDivisions(string LeagueId, string SeasonId = "");
	Task<League?>         GetLeague(string LeagueId);
	Task<Team?>           GetTeamStats(string LeagueId, string TeamName, string SeasonId = "");

	string CsvFromFixtures(ICollection<Fixture> Fixtures);

	IcalCalendar IcalFromFixtures(string LeagueId, string TeamName, ICollection<Fixture> Fixtures, TimeZoneInfo timeZone);
	string IcalStringFromFixtures(string LeagueId, string TeamName, ICollection<Fixture> Fixtures, TimeZoneInfo timeZone);
	string GetSeasonId(string seasonId, int year);
}
