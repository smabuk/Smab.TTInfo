using Smab.TTInfo.TT365.Models;

namespace Smab.TTInfo.Server.EndPoints;

public static partial class TTEndPoints
{
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

	private static async Task<Ok<List<Fixture>>> GetFixtures(ITT365Reader tt365, string leagueId, int? year, string? seasonId, string? teamName)
	{
		seasonId = await GetSeasonId(tt365, leagueId, year, seasonId);
		List<Fixture> list = await tt365.GetAllFixtures(leagueId, seasonId) ?? [];
		if (teamName is not null) {
			teamName = teamName.Replace("_", " ");
			list = list
				.Where(f => string.Equals(f.HomeTeam,teamName, StringComparison.CurrentCultureIgnoreCase) || string.Equals(f.AwayTeam, teamName, StringComparison.CurrentCultureIgnoreCase))
				.ToList();
		}

		return TypedResults.Ok(list);
	}

	private static async Task<Results<Ok<Team>, NotFound>> GetTeam(ITT365Reader tt365, string leagueId, string teamName, int? year, string seasonId = "")
	{
		teamName = teamName.Replace("_", " ");
		seasonId = await GetSeasonId(tt365, leagueId, year, seasonId);
		Team? team;
		try {
			team = await tt365.GetTeamStats(leagueId, teamName, seasonId);
		}
		catch { 
			team = null;
		}

		return team switch {
			null => TypedResults.NotFound(),
			_    => TypedResults.Ok(team),
		};
	}

	private static async Task<Results<Ok<List<string>>, NotFound>> GetTeamPlayers(ITT365Reader tt365, string leagueId, string teamName, int? year, string seasonId = "")
	{
		teamName = teamName.Replace("_", " ");
		seasonId = await GetSeasonId(tt365, leagueId, year, seasonId);
		Team? team;
		try {
			team = await tt365.GetTeamStats(leagueId, teamName, seasonId);
		}
		catch { 
			team = null;
		}

		return team switch {
			null => TypedResults.NotFound(),
			_    => TypedResults.Ok(team.Players?.Select(p => $"{p.Name} ({p.WinPercentage}%)").ToList()),
		};
	}

	private static async Task<string> GetSeasonId(ITT365Reader tt365, string leagueId, int? year, string? seasonId = "" )
	{
		if (!string.IsNullOrWhiteSpace(seasonId)) {
			return $"{seasonId}";
		}

		League? league = await tt365.GetLeague(leagueId);

		return league switch
		{
			null => "",
			_ => year switch {
					null => league.CurrentSeason.Id,
					_    => tt365.GetSeasonId(league.CurrentSeason.Id, (int)year)
				}
		};
	}
}
