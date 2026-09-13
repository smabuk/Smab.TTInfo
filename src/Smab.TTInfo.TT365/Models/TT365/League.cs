namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents a league, including its details, current season, and historical seasons.
/// </summary>
/// <remarks>A league is identified by its unique ID and contains information such as its name, description, URL,
/// and theme. It also tracks the current season and a list of all seasons associated with the league.</remarks>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="Description"></param>
/// <param name="URL"></param>
/// <param name="Theme"></param>
/// <param name="Seasons"></param>
/// <param name="CurrentSeason"></param>
public record League(
	TT365LeagueId Id,
	string Name,
	string Description,
	string URL,
	string Theme,
	ImmutableList<Season> Seasons,
	TT365SeasonId CurrentSeasonId
	);

public static class LeagueExtensions
{
	extension(League league)
	{
		/// <summary>
		/// Gets the current season of the league.
		/// </summary>
		/// <param name="league">The league from which to retrieve the current season.</param>
		/// <returns>The current season of the league.</returns>
		public Season CurrentSeason => league.Seasons.Single(s => s.Id == league.CurrentSeasonId);

		/// <summary>
		/// Retrieves a specific season from the league based on the provided season identifier.
		/// </summary>
		/// <param name="seasonId">The unique identifier of the season to retrieve.</param>
		/// <returns>The season with the specified identifier, or null if not found.</returns>
		public Season? GetSeason(TT365SeasonId seasonId) => league.Seasons.FirstOrDefault(s => s.Id == seasonId);

		/// <summary>
		/// Retrieves a specific division from the league based on the provided season identifier and division name.
		/// </summary>
		/// <param name="seasonId"></param>
		/// <param name="divisionName">The name of the division to retrieve.</param>
		/// <returns>The division with the specified name, or null if not found.</returns>
		/// <exception cref="InvalidOperationException">Thrown if the division with the specified name is not found in the season.</exception>
		public Division? GetDivisionByName(TT365SeasonId seasonId, string divisionName) =>
			league.GetSeason(seasonId)?.Divisions.FirstOrDefault(d => d.Name.Equals(divisionName.Replace("%20", " ").Replace("_", " "), StringComparison.OrdinalIgnoreCase));

		/// <summary>
		/// Retrieves a specific division from the league based on the provided season identifier and division ID.
		/// </summary>
		/// <param name="seasonId"></param>
		/// <param name="divisionId"></param>
		/// <returns>The division with the specified ID, or null if not found.</returns>
		public Division? GetDivisionById(TT365SeasonId seasonId, string divisionId) =>
			league.GetSeason(seasonId)?.Divisions.FirstOrDefault(d => d.Id == divisionId);

		/// <summary>
		/// Retrieves a specific division from the league based on the provided season identifier, division ID, or division
		/// name.
		/// </summary>
		/// <param name="seasonId"></param>
		/// <param name="divisionId"></param>
		/// <param name="divisionName"></param>
		/// <returns>The division with the specified ID or name, or null if not found.</returns>
		public Division? GetDivisionByIdOrName(TT365SeasonId seasonId, string divisionId, string divisionName) =>
			league.GetSeason(seasonId)?.Divisions.FirstOrDefault(d => d.Id == divisionId || d.Name.Equals(divisionName.Replace("%20", " ").Replace("_", " "), StringComparison.OrdinalIgnoreCase));
	}
}
