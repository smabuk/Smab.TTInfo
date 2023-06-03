namespace Smab.TTInfo.Cli;
internal class TTInfoCli
{
	public static async Task<int> Run(string leagueId, int year, string cacheFolder, string? showTeamPlayers = null, string? searchName = null)
	{
		searchName = searchName?.ToLowerInvariant() ?? null;
		showTeamPlayers = showTeamPlayers?.ToLowerInvariant() ?? null;

		List<LookupTables> allLookupTables = new();
		List<Division> allDivisions = new();

		TT365Reader tt365 = new()
		{
			CacheFolder = cacheFolder,
			CacheHours = 10_000_000,
		};

		League? league = await AnsiConsole.Status()
			.Spinner(Spinner.Known.Circle)
			.AutoRefresh(true)
			.StartAsync($"Loading... {leagueId} ...", async ctx =>
			{
				return await tt365.GetLeague(leagueId);
			}
			);

		if (league is null) {
			AnsiConsole.MarkupLine($"Couldn't get league: {leagueId}");
			return -1;
		}

		AnsiConsole.MarkupLine($"{league.Name}   {league.URL}");

		string seasonId = tt365.GetSeasonId(league.CurrentSeason.Id, year);

		AnsiConsole.MarkupLine("");
		AnsiConsole.MarkupLine($"{seasonId}");

		LookupTables lookupTables = await AnsiConsole.Status()
			.Spinner(Spinner.Known.Circle)
			.AutoRefresh(true)
			.StartAsync($"Loading... {leagueId} {seasonId} ...", async ctx =>
			{
				return await tt365.GetLookupTables(leagueId, seasonId);
			}
			);
		allLookupTables.Add(lookupTables);

		List<Division> divisions = await AnsiConsole.Status()
			.Spinner(Spinner.Known.Circle)
			.AutoRefresh(true)
			.StartAsync($"Loading... {leagueId} {seasonId} all divisions ...", async ctx =>
			{
				return await tt365.GetDivisions(leagueId, seasonId);
			}
			);
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
						.StartAsync($"Loading team... {team.Name} ...", async ctx =>
						{
							return await tt365.GetTeamStats(leagueId, team.Name, seasonId) ?? new();
						}
						);
				}
				catch (Exception ex) {
					message = $"*** {ex.Message}";
				}
				AnsiConsole.MarkupLine($"    {team.LeaguePosition,2} {team.Name,-40} {team.Points,3} {TT365Reader.FixPlayerName(newTeam.Captain),-20} {message}");
				foreach (Player player in newTeam.Players?.OrderByDescending(p => p.WinPercentage).ToList() ?? new List<Player>()) {
					bool showPlayerMatchDetails = searchName is not null && player.Name.ToLowerInvariant().Contains(searchName);
					bool showTeamDetails = showTeamPlayers is not null && newTeam.Name.ToLowerInvariant().Contains(showTeamPlayers);
					if (showTeamDetails || showPlayerMatchDetails) {
						AnsiConsole.MarkupLine($"         {TT365Reader.FixPlayerName(player.Name),-25} {player.Played,6} {(int)player.WinPercentage,3}%");
					}
					if (showPlayerMatchDetails) {
						Player p2 = await AnsiConsole.Status()
							.Spinner(Spinner.Known.Circle)
							.AutoRefresh(true)
							.StartAsync($"Loading player... {TT365Reader.FixPlayerName(player.Name)} ...", async ctx =>
							{
								return await tt365.GetPlayerStats(leagueId, player, seasonId) ?? new();
							}
							);
						foreach (PlayerResult playerResult in p2.PlayerResults.Where(pr => pr.PlayerTeamName == team.Name).OrderBy(pr => pr.Date)) {
							string dateString = playerResult.Date.ToString("dd MMM");
							dateString = dateString.Length <= 6 ? dateString : dateString[..6];
							string resultColor = GetResultColor(playerResult);
							AnsiConsole.MarkupLine($"[{resultColor}]          {dateString,-6} {(playerResult.ResultReason.Any() ? "*" : ""),1}{playerResult.Result.FirstOrDefault(),1}  {playerResult.RankingDiffString,3}  {TT365Reader.FixPlayerName(playerResult.Opponent.Name),-24}   {playerResult.OpponentTeam,-30}  {playerResult.GameScore,3}  {playerResult.Scores}[/]");
							if (playerResult.ResultReason.Any()) {
								AnsiConsole.MarkupLine($"[{resultColor}]                  {playerResult.ResultReason}[/]");
							}
						}
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
			"win"  => "green",
			"loss" => "red",
			_ => AnsiConsole.Foreground.ToString(),
		};
	}
}
