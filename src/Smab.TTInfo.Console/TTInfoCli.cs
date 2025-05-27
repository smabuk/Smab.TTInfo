using Microsoft.Extensions.Options;

namespace Smab.TTInfo.Cli;

/// <summary>
/// Provides methods for interacting with TT365 league data, including retrieving league information, player statistics,
/// team statistics, and match results.
/// </summary>
/// <remarks>This class contains static methods to perform various operations related to TT365 leagues, such as
/// fetching league details, retrieving player and team statistics, and analyzing match results. The methods rely on
/// asynchronous operations and may involve network requests to fetch data.</remarks>
internal class TTInfoCli
{
	/// <summary>
	/// Executes the process of retrieving and displaying league, season, division, team, and player statistics for a
	/// specified table tennis league and season year.
	/// </summary>
	/// <remarks>This method retrieves data from a table tennis league using the provided league identifier and
	/// season year. It displays league, season, division, team, and player statistics in the console. Optional parameters
	/// allow filtering of player and match details for specific teams, players, or opponents. The method uses caching to
	/// optimize data retrieval and displays progress using a console spinner.</remarks>
	/// <param name="ttinfoId">The unique identifier of the table tennis league to retrieve data for.</param>
	/// <param name="year">The year of the season to retrieve data for.</param>
	/// <param name="cacheFolder">The folder path where cached data is stored.</param>
	/// <param name="showTeamPlayers">An optional parameter specifying a team name. If provided, displays detailed player statistics for the specified
	/// team.</param>
	/// <param name="playerSearchName">An optional parameter specifying a player's name. If provided, displays detailed match statistics for the specified
	/// player.</param>
	/// <param name="opponentSearchName">An optional parameter specifying an opponent's name. If provided, filters the player's match statistics to include
	/// only matches against the specified opponent.</param>
	/// <returns>A task that represents the asynchronous operation. The task result is an integer indicating the success or failure
	/// of the operation: <list type="bullet"> <item><description>Returns 0 if the operation completes
	/// successfully.</description></item> <item><description>Returns -1 if the league data could not be
	/// retrieved.</description></item> </list></returns>
	public static async Task<int> Run(string ttinfoId, int year, string cacheFolder, string? showTeamPlayers = null, string? playerSearchName = null, string? opponentSearchName = null)
	{
		playerSearchName   = playerSearchName?.ToLowerInvariant()   ?? null;
		opponentSearchName = opponentSearchName?.ToLowerInvariant() ?? null;
		showTeamPlayers    = showTeamPlayers?.ToLowerInvariant()    ?? null;

		List<Division>     allDivisions    = [];
		List<LookupTables> allLookupTables = [];

		TT365Options ttInfoOptions = new()
		{
			CacheFolder = cacheFolder,
			CacheHours = 10_000_000,
		};

		AnsiConsole.MarkupLine($"Cache Folder: [green]{ttInfoOptions.CacheFolder}[/]");
		AnsiConsole.MarkupLine("");
		
		TT365Reader tt365 = new(Options.Create(ttInfoOptions), new HttpClient(), TimeProvider.System);

		League? league = await AnsiConsole.Status()
			.Spinner(Spinner.Known.Circle)
			.AutoRefresh(true)
			.StartAsync($"Loading... {ttinfoId} ...", async ctx => await tt365.GetLeague((TT365LeagueId)ttinfoId));

		if (league is null) {
			AnsiConsole.MarkupLine($"Couldn't get league: {ttinfoId}");
			return -1;
		}

		AnsiConsole.MarkupLine($"{league.Name}   {league.URL}");

		TT365SeasonId seasonId = tt365.GetSeasonId(league.CurrentSeason.Id, year);

		AnsiConsole.MarkupLine("");
		AnsiConsole.MarkupLine($"{seasonId}");

		LookupTables lookupTables = await AnsiConsole.Status()
			.Spinner(Spinner.Known.Circle)
			.AutoRefresh(true)
			.StartAsync($"Loading... {ttinfoId} {seasonId} ...", async ctx => await tt365.GetLookupTables((TT365LeagueId)ttinfoId, seasonId));
		allLookupTables.Add(lookupTables);

		List<Division> divisions = await AnsiConsole.Status()
			.Spinner(Spinner.Known.Circle)
			.AutoRefresh(true)
			.StartAsync($"Loading... {ttinfoId} {seasonId} all divisions ...", async ctx => await tt365.GetDivisions((TT365LeagueId)ttinfoId, seasonId));
		allDivisions.AddRange(divisions);

		foreach (Division division in divisions) {
			AnsiConsole.MarkupLine("");
			AnsiConsole.MarkupLine($"  {division.Name}");
			foreach (Team team in division.Teams) {
				Team newTeam = new();
				string message = "";
				try {
					newTeam = await AnsiConsole.Status()
						.Spinner(Spinner.Known.Circle)
						.AutoRefresh(true)
						.StartAsync($"Loading team... {team.Name} ...", async ctx => await tt365.GetTeamStats((TT365LeagueId)ttinfoId, team.Name, seasonId) ?? new());
				}
				catch (Exception ex) {
					message = $"*** {ex.Message}";
				}

				AnsiConsole.MarkupLine($"    {team.LeaguePosition,2} {team.Name,-40} {team.Points,3} {TT365Reader.FixPlayerName(newTeam.Captain),-20} {message}");
				foreach (Player player in newTeam.Players?.OrderByDescending(p => p.WinPercentage).ToList() ?? []) {
					bool showPlayerMatchDetails = playerSearchName is not null && player.Name.Contains(playerSearchName, StringComparison.InvariantCultureIgnoreCase);
					bool showTeamDetails = showTeamPlayers is not null && newTeam.Name.Contains(showTeamPlayers, StringComparison.InvariantCultureIgnoreCase);
					if (showTeamDetails || showPlayerMatchDetails) {
						AnsiConsole.MarkupLine($"         {TT365Reader.FixPlayerName(player.Name),-25} {player.Played,6} {(int)player.WinPercentage,3}%");
					};
					if (showPlayerMatchDetails) {
						Player p2 = await AnsiConsole.Status()
							.Spinner(Spinner.Known.Circle)
							.AutoRefresh(true)
							.StartAsync($"Loading player... {TT365Reader.FixPlayerName(player.Name)} ...", async ctx => await tt365.GetPlayerStats((TT365LeagueId)ttinfoId, player, seasonId) ?? new());
						foreach (PlayerResult playerResult in p2.PlayerResults.Where(pr => pr.PlayerTeamName == team.Name && (opponentSearchName is null || pr.Opponent.Name.Contains(opponentSearchName, StringComparison.InvariantCultureIgnoreCase))).OrderBy(pr => pr.Date)) {
							//bool limitToOpponentMatchDetails = opponentSearchName is null || playerResult.Opponent.Name.ToLowerInvariant().Contains(opponentSearchName);
							string dateString = playerResult.Date.ToString("dd MMM yy").Replace("Sept", "Sep");
							dateString = dateString.Length <= 9 ? dateString : dateString[..9];
							string resultColor = GetResultColor(playerResult);
							AnsiConsole.MarkupLine($"[{resultColor}]          {dateString,-9} {(playerResult.ResultReason.Length != 0 ? "*" : ""),1}{playerResult.Result.FirstOrDefault(),1}  {playerResult.FormattedRankingDiff,3}  {TT365Reader.FixPlayerName(playerResult.Opponent.Name),-24}   {playerResult.OpponentTeam,-30}  {playerResult.GameScore,3}  {playerResult.Scores}[/]");
							if (playerResult.ResultReason.Length != 0) {
								AnsiConsole.MarkupLine($"[{resultColor}]                  {playerResult.ResultReason}[/]");
							}
						}
					}
				}
			}
		}

		return 0;
	}

	/// <summary>
	/// Retrieves and displays the match results between a specified player and their opponents across multiple seasons in
	/// a table tennis league.
	/// </summary>
	/// <remarks>This method queries a table tennis league for match data involving a specific player and their
	/// opponents. It iterates through seasons starting from the specified year down to 2011, retrieving and displaying
	/// match results in a formatted output. The method uses caching to optimize data retrieval.</remarks>
	/// <param name="ttinfoId">The unique identifier of the table tennis league to query.</param>
	/// <param name="year">The starting year for the search, which will iterate backward to 2011.</param>
	/// <param name="cacheFolder">The folder path used for caching league data.</param>
	/// <param name="playerSearchName">The name of the player to search for, case-insensitive.</param>
	/// <param name="opponentSearchName">The name of the opponent to search for, case-insensitive.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains an integer indicating the status of the
	/// operation: 0 if successful, or -1 if the league could not be retrieved.</returns>
	public static async Task<int> PlayerVsPlayer(string ttinfoId, int year, string cacheFolder, string playerSearchName, string opponentSearchName)
	{
		playerSearchName = playerSearchName.ToLowerInvariant();
		opponentSearchName = opponentSearchName.ToLowerInvariant();

		HashSet<SeasonPlayer> players = [];

		TT365Options ttInfoOptions = new()
		{
			CacheFolder = cacheFolder,
			CacheHours = 10_000_000,
		};

		AnsiConsole.MarkupLine($"Cache Folder: [green]{ttInfoOptions.CacheFolder}[/]");
		AnsiConsole.MarkupLine("");

		TT365Reader tt365 = new(Options.Create(ttInfoOptions), new HttpClient(), TimeProvider.System);

		League? league = await AnsiConsole.Status()
			.Spinner(Spinner.Known.Circle)
			.AutoRefresh(true)
			.StartAsync($"Loading... {ttinfoId} ...", async ctx => await tt365.GetLeague((TT365LeagueId)ttinfoId));

		if (league is null) {
			AnsiConsole.MarkupLine($"Couldn't get league: {ttinfoId}");
			return -1;
		}

		AnsiConsole.MarkupLine($"{league.Name}   {league.URL}");

		await AnsiConsole.Status()
			.Spinner(Spinner.Known.Circle)
			.AutoRefresh(true)
			.StartAsync($"Loading... {ttinfoId} ...", async ctx =>
			{
				for (int iYear = year; iYear > 2011; iYear--) {
					string seasonId = tt365.GetSeasonId(league.CurrentSeason.Id, iYear);

					List<Fixture>? fixtures = await tt365.GetAllFixtures((TT365LeagueId)ttinfoId, (TT365SeasonId)seasonId);
					if (fixtures is not null) {
						foreach (CompletedFixture fixture in fixtures.OfType<CompletedFixture>()) {
							foreach (MatchPlayer matchPlayer in fixture.HomePlayers.Where(p => p.Name.Contains(playerSearchName, StringComparison.InvariantCultureIgnoreCase))) {
								_ = players.Add(new SeasonPlayer(matchPlayer.Name, matchPlayer.Id, seasonId));
							}

							foreach (MatchPlayer matchPlayer in fixture.AwayPlayers.Where(p => p.Name.Contains(playerSearchName, StringComparison.InvariantCultureIgnoreCase))) {
								_ = players.Add(new SeasonPlayer(matchPlayer.Name, matchPlayer.Id, seasonId));
							}
						}
					}
				}
			});

		foreach (string playerName in players.DistinctBy(p => p.Name).OrderBy(p => p.Name).Select(p => p.Name)) {
			AnsiConsole.MarkupLine($" {TT365Reader.FixPlayerName(playerName),-25}");

			foreach (SeasonPlayer p1 in players.Where(p => p.Name == playerName).OrderBy(p => p.PlayerId)) {
				Player p2 = await AnsiConsole.Status()
					.Spinner(Spinner.Known.Circle)
					.AutoRefresh(true)
					.StartAsync($"Loading player... {p1.SeasonId} {TT365Reader.FixPlayerName(playerName)} ...", async ctx =>
					{
						Player player1 = new()
						{
							Name = p1.Name,
							PlayerId = p1.PlayerId,
						};
						return await tt365.GetPlayerStats((TT365LeagueId)ttinfoId, player1, (TT365SeasonId?)p1.SeasonId) ?? new();
					});

				foreach (PlayerResult playerResult in p2.PlayerResults.Where(pr => pr.Opponent.Name.Contains(opponentSearchName, StringComparison.InvariantCultureIgnoreCase)).OrderBy(pr => pr.Date)) {
					string dateString = playerResult.Date.ToString("dd MMM yy").Replace("Sept", "Sep");
					dateString = dateString.Length <= 9 ? dateString : dateString[..9];
					string resultColor = GetResultColor(playerResult);
					AnsiConsole.MarkupLine($"[{resultColor}]   {dateString,-9}  Div {playerResult.Division[^1]} {(playerResult.ResultReason.Length != 0 ? "*" : ""),1}{playerResult.Result.FirstOrDefault(),1}  {playerResult.FormattedRankingDiff,3}  {TT365Reader.FixPlayerName(playerResult.Opponent.Name),-24}   {playerResult.OpponentTeam,-30}  {playerResult.GameScore,3}  {playerResult.Scores}[/]");
					if (playerResult.ResultReason.Length != 0) {
						AnsiConsole.MarkupLine($"                     [{resultColor}]{playerResult.ResultReason}[/]");
					}
				}
			}
		}

		return 0;
	}

	/// <summary>
	/// Determines the color representation for a player's result.
	/// </summary>
	/// <param name="playerResult">The player's result, which must contain a <see cref="PlayerResult.Result"/> value indicating the outcome (e.g.,
	/// "win" or "loss").</param>
	/// <returns>A string representing the color associated with the player's result.  Returns "green" for a win, "red" for a loss,
	/// or the current console foreground color for any other result.</returns>
	private static string GetResultColor(PlayerResult playerResult)
	{
		return playerResult.Result.ToLowerInvariant() switch
		{
			"win"  => "green",
			"loss" => "red",
			_ => AnsiConsole.Foreground.ToString(),
		};
	}

	/// <summary>
	/// Represents a player participating in a specific season.
	/// </summary>
	/// <remarks>This record encapsulates the player's name, unique identifier, and the season they are associated
	/// with. It is intended to provide a lightweight, immutable representation of a player's seasonal
	/// participation.</remarks>
	/// <param name="Name"></param>
	/// <param name="PlayerId"></param>
	/// <param name="SeasonId"></param>
	private record SeasonPlayer(string Name, int PlayerId, string SeasonId);
}
