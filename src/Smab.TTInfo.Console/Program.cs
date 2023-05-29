const string leagueId = "Reading";
int year = DateTime.Today.Year;

List<LookupTables> allLookupTables = new();
List<Division> allDivisions = new();

Console.WriteLine("Table Tennis 365 reader");

if (args.Length == 1) {
	year = int.Parse(args[0]);
}

TT365Reader tt365 = new()
{
	CacheFolder = """c:\temp\tt365\cache""",
	CacheHours = 1000,
};

League? league = await tt365.GetLeague(leagueId);
if (league is null) {
	Console.WriteLine($"Couldn't get league: {leagueId}");
	return;
}

Console.WriteLine($"{league.Name}   {league.URL}");


string seasonId = $"Senior_{year}-{year + 1 - 2000}";
Console.WriteLine();
Console.WriteLine($"{seasonId}");
LookupTables lookupTables = await tt365.GetLookupTables(leagueId, seasonId);
allLookupTables.Add(lookupTables);
List<Division> divisions = await tt365.GetDivisions(leagueId, seasonId);
allDivisions.AddRange(divisions);
foreach (Division division in divisions) {
	Console.WriteLine($"  {division.Name}");
	foreach (Team team in division.Teams) {
		Team newTeam = new();
		Console.Write($"    {team.LeaguePosition, 2} {team.Name,-40} {team.Points,3}");
		try {
			newTeam = await tt365.GetTeamStats(leagueId, team.Name, seasonId) ?? new();
		}
		catch (Exception ex) {
			Console.Write($" *** {ex.Message}");
		}
		finally {
			Console.WriteLine($"  {newTeam.Captain,-20}");
		}
		if (newTeam.Name.ToLowerInvariant().Contains("peace")) {
			foreach (Player player in newTeam.Players?.OrderByDescending(p => p.WinPercentage).ToList() ?? new List<Player>()) {
				Console.WriteLine($"         {player.Name,-25} {player.Played,6} {(int)player.WinPercentage,3}%");
			}
		}
	}
}

