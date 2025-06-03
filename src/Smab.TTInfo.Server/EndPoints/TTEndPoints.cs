namespace Smab.TTInfo.Server.EndPoints;

/// <summary>
/// Provides extension methods for mapping TT (Table Tennis) API endpoints to a route group or application.
/// </summary>
/// <remarks>This class contains methods to register API endpoints for retrieving fixtures, team details, and team
/// players. The endpoints are mapped to specific routes and support various parameter combinations, such as league ID,
/// team name, season ID, and year.</remarks>
public static partial class TTEndPoints
{
	/// <summary>
	/// Maps a set of endpoints related to fixtures, teams, and team players to the specified route group.
	/// </summary>
	/// <remarks>This method registers multiple GET endpoints for retrieving fixtures, team details, and team
	/// players. The endpoints support various route patterns, including optional parameters for season or year.</remarks>
	/// <param name="group">The <see cref="RouteGroupBuilder"/> to which the endpoints will be added.</param>
	/// <returns>The <see cref="RouteGroupBuilder"/> with the mapped endpoints.</returns>
	public static RouteGroupBuilder MapTTEndPoints(this RouteGroupBuilder group)
	{
		_ = group.MapGet("/fixtures/{leagueId}",                       GetFixtures);
		_ = group.MapGet("/fixtures/{leagueId}/{teamname}",            GetFixtures);
		_ = group.MapGet("/fixtures/{seasonId}/{leagueId}/{teamName}", GetFixtures);
		_ = group.MapGet("/fixtures/{year:int}/{leagueId}/{teamName}", GetFixtures);

		_ = group.MapGet("/team/{leagueId}/{teamName}",            GetTeam);
		_ = group.MapGet("/team/{seasonId}/{leagueId}/{teamName}", GetTeam);
		_ = group.MapGet("/team/{year:int}/{leagueId}/{teamName}", GetTeam);

		_ = group.MapGet("/teamplayers/{leagueId}/{teamName}",            GetTeamPlayers);
		_ = group.MapGet("/teamplayers/{seasonId}/{leagueId}/{teamName}", GetTeamPlayers);
		_ = group.MapGet("/teamplayers/{year:int}/{leagueId}/{teamName}", GetTeamPlayers);

		return group;
	}

	/// <summary>
	/// Configures the application's HTTP endpoints for accessing fixtures, teams, and team players.
	/// </summary>
	/// <remarks>This method defines multiple HTTP GET endpoints for retrieving data related to fixtures, teams, and
	/// team players. The endpoints support various route patterns, allowing queries by league, team name, season, or
	/// year.</remarks>
	/// <param name="app">The <see cref="WebApplication"/> instance to which the endpoints will be mapped. Must not be null.</param>
	public static void MapTTEndPoints(this WebApplication? app)
	{
		_ = app?.MapGet("/api/fixtures/{leagueId}",                       GetFixtures);
		_ = app?.MapGet("/api/fixtures/{leagueId}/{teamName}",            GetFixtures);
		_ = app?.MapGet("/api/fixtures/{seasonId}/{leagueId}/{teamName}", GetFixtures);

		_ = app?.MapGet("/api/team/{leagueId}/{teamName}",            GetTeam);
		_ = app?.MapGet("/api/team/{seasonId}/{leagueId}/{teamName}", GetTeam);
		_ = app?.MapGet("/api/team/{year:int}/{leagueId}/{teamName}", GetTeam);

		_ = app?.MapGet("/api/teamplayers/{leagueId}/{teamName}",            GetTeamPlayers);
		_ = app?.MapGet("/api/teamplayers/{seasonId}/{leagueId}/{teamName}", GetTeamPlayers);
		_ = app?.MapGet("/api/teamplayers/{year:int}/{leagueId}/{teamName}", GetTeamPlayers);
	}

	/// <summary>
	/// Retrieves a list of fixtures for a specified league and season, optionally filtered by team name.
	/// </summary>
	/// <remarks>This method fetches all fixtures for the specified league and season, and optionally filters them
	/// by the provided team name. If <paramref name="teamName"/> is specified, the method performs a case-insensitive
	/// comparison to match the team name.</remarks>
	/// <param name="tt365">An instance of <see cref="ITT365Reader"/> used to fetch league and fixture data.</param>
	/// <param name="leagueId">The unique identifier of the league for which fixtures are retrieved.</param>
	/// <param name="year">The year associated with the season. If <paramref name="seasonId"/> is not provided, this parameter is used to
	/// determine the season.</param>
	/// <param name="seasonId">The unique identifier of the season. If null, the method will attempt to determine the season based on the
	/// <paramref name="year"/>.</param>
	/// <param name="teamName">The name of the team to filter fixtures by. If specified, only fixtures involving this team (as the home or away
	/// team) are returned. The team name is case-insensitive and underscores (_) are treated as spaces.</param>
	/// <returns>An <see cref="Ok{T}"/> result containing a list of <see cref="Fixture"/> objects. If no fixtures are found, an
	/// empty list is returned.</returns>
	private static async Task<Ok<List<Fixture>>> GetFixtures(ITT365Reader tt365, string leagueId, int? year, string? seasonId, string? teamName)
	{
		seasonId = await GetSeasonId(tt365, leagueId, year, seasonId);
		List<Fixture> list = await tt365.GetAllFixtures((TT365LeagueId)leagueId, (TT365SeasonId?)seasonId) ?? [];
		if (teamName is not null) {
			teamName = teamName.Replace("_", " ");
			list = [.. list.Where(f => string.Equals(f.HomeTeam,teamName, StringComparison.CurrentCultureIgnoreCase) || string.Equals(f.AwayTeam, teamName, StringComparison.CurrentCultureIgnoreCase))];
		}

		return TypedResults.Ok(list);
	}

	/// <summary>
	/// Retrieves a team's statistics for a specified league, team name, and season.
	/// </summary>
	/// <remarks>This method attempts to retrieve the team's statistics for the specified league, team name, and
	/// season. If the team cannot be found or an error occurs during data retrieval, a <see cref="NotFound"/> result is
	/// returned.</remarks>
	/// <param name="tt365">An instance of <see cref="ITT365Reader"/> used to fetch team data.</param>
	/// <param name="leagueId">The unique identifier of the league to which the team belongs. Cannot be null or empty.</param>
	/// <param name="teamName">The name of the team to retrieve statistics for. Underscores in the name will be replaced with spaces.</param>
	/// <param name="year">The year of the season to retrieve statistics for. If null, the current year is used.</param>
	/// <param name="seasonId">An optional identifier for the season. If not provided, the method will attempt to determine the season ID based on
	/// the league and year.</param>
	/// <returns>A <see cref="Results{T1, T2}"/> object containing either: <list type="bullet"> <item><description>An <see
	/// cref="Ok{T}"/> result with the team's statistics if found.</description></item> <item><description>A <see
	/// cref="NotFound"/> result if the team or season data could not be retrieved.</description></item> </list></returns>
	private static async Task<Results<Ok<Team>, NotFound>> GetTeam(ITT365Reader tt365, string leagueId, string teamName, int? year, string seasonId = "")
	{
		teamName = teamName.Replace("_", " ");
		seasonId = await GetSeasonId(tt365, leagueId, year, seasonId);
		Team? team;
		try {
			team = await tt365.GetTeamStats((TT365LeagueId)leagueId, teamName, (TT365SeasonId)seasonId);
		}
		catch { 
			team = null;
		}

		return team switch {
			null => TypedResults.NotFound(),
			_    => TypedResults.Ok(team),
		};
	}

	/// <summary>
	/// Retrieves a list of players for a specified team in a given league and season.
	/// </summary>
	/// <remarks>This method fetches the team statistics and extracts the list of players along with their win
	/// percentages. If the team is not found, a <see cref="NotFound"/> result is returned.</remarks>
	/// <param name="tt365">An instance of <see cref="ITT365Reader"/> used to fetch team and league data.</param>
	/// <param name="leagueId">The unique identifier of the league.</param>
	/// <param name="teamName">The name of the team. Underscores in the name will be replaced with spaces.</param>
	/// <param name="year">The year of the season. If null, the current year is used.</param>
	/// <param name="seasonId">The unique identifier of the season. If not provided, it will be determined based on the league and year.</param>
	/// <returns>A <see cref="Results{T1, T2}"/> object containing one of the following: <list type="bullet"> <item> <description>
	/// <see cref="Ok{T}"/>: A list of player names with their win percentages in the format "Name (WinPercentage%)".
	/// </description> </item> <item> <description> <see cref="NotFound"/>: Indicates that the specified team could not be
	/// found. </description> </item> </list></returns>
	private static async Task<Results<Ok<List<string>>, NotFound>> GetTeamPlayers(ITT365Reader tt365, string leagueId, string teamName, int? year, string seasonId = "")
	{
		teamName = teamName.Replace("_", " ");
		seasonId = await GetSeasonId(tt365, leagueId, year, seasonId);
		Team? team;
		try {
			team = await tt365.GetTeamStats((TT365LeagueId)leagueId, teamName, (TT365SeasonId)seasonId);
		}
		catch { 
			team = null;
		}

		return team switch {
			null => TypedResults.NotFound(),
			_    => TypedResults.Ok(team.Players?.Select(p => $"{p.Name} ({p.WinPercentage}%)").ToList()),
		};
	}

	/// <summary>
	/// Retrieves the season identifier based on the provided parameters.
	/// </summary>
	/// <remarks>If a valid <paramref name="seasonId"/> is provided, it is returned directly without further
	/// processing. Otherwise, the method retrieves the league using the <paramref name="leagueId"/> and determines the
	/// season ID based on the specified <paramref name="year"/> or the current season if <paramref name="year"/> is <see
	/// langword="null"/>.</remarks>
	/// <param name="tt365">An instance of <see cref="ITT365Reader"/> used to fetch league and season information.</param>
	/// <param name="leagueId">The unique identifier of the league for which the season ID is being retrieved.</param>
	/// <param name="year">The specific year for which the season ID is being retrieved. If <see langword="null"/>, the current season ID is
	/// returned.</param>
	/// <param name="seasonIdAsString">An optional pre-existing season ID. If provided and not empty, it will be returned as-is.</param>
	/// <returns>A <see cref="TT365SeasonId?"/> representing the season ID. Returns <see langword="null"/> if the league cannot be found.</returns>
	private static async Task<TT365SeasonId?> GetSeasonId(ITT365Reader tt365, string leagueId, int? year, string? seasonIdAsString = null )
	{
		if (!string.IsNullOrWhiteSpace(seasonIdAsString)) {
			return new(seasonIdAsString);
		}

		League? league = await tt365.GetLeague((TT365LeagueId)leagueId);

		return league switch
		{
			null => null,
			_ => year switch {
					null => league.GetCurrentSeasonId(),
					_    => tt365.GetSeasonId(league.GetCurrentSeasonId(), (int)year)
				}
		};
	}
}
