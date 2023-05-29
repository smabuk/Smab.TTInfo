namespace Smab.TTInfo.Server.EndPoints;

public static partial class TTEndPoints
{

	public static void MapTTEndPoints(this WebApplication? app)
	{
		app?.MapGet("/api/fixtures/{leagueId}",                       GetFixtures);
		app?.MapGet("/api/fixtures/{leagueId}/{teamName}",            GetFixtures);
		app?.MapGet("/api/fixtures/{seasonId}/{leagueId}/{teamName}", GetFixtures);

		app?.MapGet("/api/team/{leagueId}/{teamName}",            GetTeam);
		app?.MapGet("/api/team/{seasonId}/{leagueId}/{teamName}", GetTeam);

		app?.MapGet("/api/teamplayers/{leagueId}/{teamName}",            GetTeamPlayers);
		app?.MapGet("/api/teamplayers/{seasonId}/{leagueId}/{teamName}", GetTeamPlayers);
	}

	private static async Task<Ok<List<Fixture>>> GetFixtures(ITT365Reader tt365, string leagueId, string? seasonId, string? teamName)
	{
		List<Fixture> list = await tt365.GetAllFixtures(leagueId, seasonId) ?? new();
		if (teamName is not null) {
			teamName = teamName.Replace("_", " ");
			list = list
				.Where(f => string.Equals(f.HomeTeam,teamName, StringComparison.CurrentCultureIgnoreCase) || string.Equals(f.AwayTeam, teamName, StringComparison.CurrentCultureIgnoreCase))
				.ToList();
		}

		return TypedResults.Ok(list);
	}

	private static async Task<Results<Ok<Team>, NotFound>> GetTeam(ITT365Reader tt365, string leagueId, string teamName, string seasonId = "")
	{
		teamName = teamName.Replace("_", " ");
		Team? team;
		try {
			team = await tt365.GetTeamStats(leagueId, teamName, seasonId);
		}
		catch { 
			team = null;
		}

		return team switch
		{
			null => TypedResults.NotFound(),
			_    => TypedResults.Ok(team),
		};
	}

	private static async Task<Results<Ok<List<string>>, NotFound>> GetTeamPlayers(ITT365Reader tt365, string leagueId, string teamName, string seasonId = "")
	{
		teamName = teamName.Replace("_", " ");
		Team? team;
		try {
			team = await tt365.GetTeamStats(leagueId, teamName, seasonId);
		}
		catch { 
			team = null;
		}

		return team switch
		{
			null => TypedResults.NotFound(),
			_    => TypedResults.Ok(team.Players?.Select(p => $"{p.Name} ({p.WinPercentage})").ToList()),
		};
	}
}
