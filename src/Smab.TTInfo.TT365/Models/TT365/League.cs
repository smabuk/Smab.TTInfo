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
	string Id,
	string Name,
	string Description,
	string URL,
	string Theme,
	List<Season> Seasons,
	Season CurrentSeason
	);

public static class LeagueExtensions
{
	/// <summary>
	/// Gets the current season of the league.
	/// </summary>
	/// <param name="league">The league from which to retrieve the current season.</param>
	/// <returns>The current season of the league.</returns>
	public static Season GetCurrentSeason(this League league) => league.CurrentSeason;
}
