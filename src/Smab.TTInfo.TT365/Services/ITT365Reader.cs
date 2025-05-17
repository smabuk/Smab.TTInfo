namespace Smab.TTInfo.TT365.Services;

/// <summary>
/// Defines the contract for reading TT365 league, team, player, and fixture data.
/// </summary>
public interface ITT365Reader
{
	/// <summary>
	/// Gets the season ID for a given year.
	/// </summary>
	string GetSeasonId(string seasonId, int year);

	/// <summary>
	/// Gets all fixtures for a league and optional season.
	/// </summary>
	Task<List<Fixture>>   GetAllFixtures(string ttInfoId, string? SeasonId = null);
	/// <summary>
	/// Gets all divisions for a league and season.
	/// </summary>
	Task<List<Division>>  GetDivisions(string ttInfoId, string SeasonId = "");
	/// <summary>
	/// Gets the league details for a given league ID.
	/// </summary>
	Task<League?>         GetLeague(string ttInfoId);
	/// <summary>
	/// Gets player statistics for a player and season.
	/// </summary>
	Task<Player?>         GetPlayerStats(string ttInfoId, Player player, string SeasonId = "");
	/// <summary>
	/// Gets team statistics for a team and season.
	/// </summary>
	Task<Team?>           GetTeamStats(string ttInfoId, string TeamName, string SeasonId = "");

	/// <summary>
	/// Generates a CSV string from a collection of fixtures.
	/// </summary>
	string CsvFromFixtures(ICollection<Fixture> Fixtures);
	/// <summary>
	/// Generates an iCal calendar from a collection of fixtures.
	/// </summary>
	IcalCalendar IcalFromFixtures(string ttInfoId, string TeamName, ICollection<Fixture> Fixtures, TimeZoneInfo timeZone);
	/// <summary>
	/// Generates an iCal string from a collection of fixtures.
	/// </summary>
	string IcalStringFromFixtures(string ttInfoId, string TeamName, ICollection<Fixture> Fixtures, TimeZoneInfo timeZone);
}
