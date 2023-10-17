namespace Smab.TTInfo.TT365.Services;

public interface ITT365Reader
{
	Task<List<Fixture>?>  GetAllFixtures(string ttInfoId, string? SeasonId = null);

	Task<List<Division>>  GetDivisions(string ttInfoId, string SeasonId = "");
	Task<League?>         GetLeague(string ttInfoId);
	Task<Player?>         GetPlayerStats(string ttInfoId, Player player, string SeasonId = "");
	Task<Team?>           GetTeamStats(string ttInfoId, string TeamName, string SeasonId = "");

	string CsvFromFixtures(ICollection<Fixture> Fixtures);

	IcalCalendar IcalFromFixtures(string ttInfoId, string TeamName, ICollection<Fixture> Fixtures, TimeZoneInfo timeZone);
	string IcalStringFromFixtures(string ttInfoId, string TeamName, ICollection<Fixture> Fixtures, TimeZoneInfo timeZone);
	string GetSeasonId(string seasonId, int year);
}
