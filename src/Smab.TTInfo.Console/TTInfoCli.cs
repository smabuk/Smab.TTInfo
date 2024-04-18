using Microsoft.Extensions.Options;

namespace Smab.TTInfo.Cli;
internal class TTInfoCli
{
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
			.StartAsync($"Loading... {ttinfoId} ...", async ctx => await tt365.GetLeague(ttinfoId));

		if (league is null) {
			AnsiConsole.MarkupLine($"Couldn't get league: {ttinfoId}");
			return -1;
		}

		AnsiConsole.MarkupLine($"{league.Name}   {league.URL}");

		string seasonId = tt365.GetSeasonId(league.CurrentSeason.Id, year);

		AnsiConsole.MarkupLine("");
		AnsiConsole.MarkupLine($"{seasonId}");

		LookupTables lookupTables = await AnsiConsole.Status()
			.Spinner(Spinner.Known.Circle)
			.AutoRefresh(true)
			.StartAsync($"Loading... {ttinfoId} {seasonId} ...", async ctx => await tt365.GetLookupTables(ttinfoId, seasonId));
		allLookupTables.Add(lookupTables);

		List<Division> divisions = await AnsiConsole.Status()
			.Spinner(Spinner.Known.Circle)
			.AutoRefresh(true)
			.StartAsync($"Loading... {ttinfoId} {seasonId} all divisions ...", async ctx => await tt365.GetDivisions(ttinfoId, seasonId));
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
						.StartAsync($"Loading team... {team.Name} ...", async ctx => await tt365.GetTeamStats(ttinfoId, team.Name, seasonId) ?? new());
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
							.StartAsync($"Loading player... {TT365Reader.FixPlayerName(player.Name)} ...", async ctx => await tt365.GetPlayerStats(ttinfoId, player, seasonId) ?? new());
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
			.StartAsync($"Loading... {ttinfoId} ...", async ctx => await tt365.GetLeague(ttinfoId));

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

					List<Fixture>? fixtures = await tt365.GetAllFixtures(ttinfoId, seasonId);
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
						return await tt365.GetPlayerStats(ttinfoId, player1, p1.SeasonId) ?? new();
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

	private static string GetResultColor(PlayerResult playerResult)
	{
		return playerResult.Result.ToLowerInvariant() switch
		{
			"win" => "green",
			"loss" => "red",
			_ => AnsiConsole.Foreground.ToString(),
		};
	}

	private record SeasonPlayer(string Name, int PlayerId, string SeasonId);
}
