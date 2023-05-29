const string leagueId = "Reading";
List<LookupTables> allLookupTables = new();
List<Division> allDivisions = new();

Console.WriteLine("Table Tennis 365 reader");

TT365Reader tt365 = new()
{
	CacheFolder = """c:\temp\tt365\cache""",
	CacheHours = 100,
};

League? league = await tt365.GetLeague(leagueId);

Console.WriteLine(league?.URL);

for (int year = 17; year < 23; year++) {
	string seasonId = $"Senior_20{year}-{year + 1}";
	Console.WriteLine();
	Console.WriteLine($"Getting {leagueId} - {seasonId}");
	LookupTables lookupTables = await tt365.GetLookupTables(leagueId, seasonId);
	allLookupTables.Add(lookupTables);
	List<Division> divisions = await tt365.GetDivisions(leagueId, seasonId);
	allDivisions.AddRange(divisions);
	foreach (Division division in divisions) {
		Console.WriteLine($"  {division.Name}");
		foreach (Team team in division.Teams) {
			Console.WriteLine($"    {team.LeaguePosition, 2} {team.Name,-40} {team.Points,3}");
		}
	}
}

