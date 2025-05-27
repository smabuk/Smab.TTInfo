namespace Smab.TTInfo.TT365.Interfaces;

/// <summary>
/// Defines the contract for reading TT365 league, team, player, and fixture data.
/// </summary>
public interface ITT365Reader
{
	/// <summary>
	/// Gets the season ID for a given year.
	/// </summary>
	TT365SeasonId GetSeasonId(string seasonId, int year);

	/// <summary>
	/// Gets all fixtures for a league and optional season.
	/// </summary>
	Task<List<Fixture>>   GetAllFixtures(TT365LeagueId leagueId, TT365SeasonId? SeasonId = null);
	/// <summary>
	/// Gets all divisions for a league and season.
	/// </summary>
	Task<List<Division>>  GetDivisions(TT365LeagueId leagueId, TT365SeasonId SeasonId);
	/// <summary>
	/// Gets the league details for a given league ID.
	/// </summary>
	Task<League?>         GetLeague(TT365LeagueId leagueId);
	/// <summary>
	/// Gets player statistics for a player and season.
	/// </summary>
	Task<Player?>         GetPlayerStats(TT365LeagueId leagueId, Player player, TT365SeasonId? SeasonId = null);
	/// <summary>
	/// Gets team statistics for a team and season.
	/// </summary>
	Task<Team?>           GetTeamStats(TT365LeagueId leagueId, string TeamName, TT365SeasonId? SeasonId = null);

	/// <summary>
	/// Generates a CSV string from a collection of fixtures.
	/// </summary>
	string CsvFromFixtures(ICollection<Fixture> Fixtures);
	/// <summary>
	/// Generates an iCal calendar from a collection of fixtures.
	/// </summary>
	IcalCalendar IcalFromFixtures(TT365LeagueId leagueId, string TeamName, ICollection<Fixture> Fixtures, TimeZoneInfo timeZone);
	/// <summary>
	/// Generates an iCal string from a collection of fixtures.
	/// </summary>
	string IcalStringFromFixtures(TT365LeagueId leagueId, string TeamName, ICollection<Fixture> Fixtures, TimeZoneInfo timeZone);
}
