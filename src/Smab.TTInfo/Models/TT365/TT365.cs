using System.Diagnostics;

namespace Smab.TTInfo.Models.TT365;

public static class FixturesViewType
{
	public const int Advanced = 1;
	public const int Simple = 2;
	public const int Chart = 3;
	public const int Print = 4;
	public const int Export = 5;
}

public class FixturesView
{
	public string Caption { get; set; } = "";
	public string URL { get; set; } = "";
	public FixturesViewOptions Options { get; set; } = new();
	public ICollection<Fixture>? Fixtures { get; set; }
}

public record FixturesViewOptions
{
	public string Season { get; set; } = "";
	public string Division { get; set; } = "All Divisions";
	public string ClubId { get; set; } = "";
	public string TeamId { get; set; } = "";
	public string VenueId { get; set; } = "";
	public int ViewModeType { get; set; } = FixturesViewType.Advanced;
	public bool HideCompletedFixtures { get; set; } = false;
	public bool MergeDivisions { get; set; } = true;
	public bool ShowByWeekNo { get; set; } = true;
}


[DebuggerDisplay("Fixture: {Date,nq} - {HomeTeam,nq} vs {AwayTeam,nq}")]
public class Fixture
{
	public string Division { get; set; } = "";
	public string Description { get; set; } = "";
	public DateOnly Date { get; set; }
	public string HomeTeam { get; set; } = "";
	public string AwayTeam { get; set; } = "";
	public string Venue { get; set; } = "";
	public bool IsCompleted { get; set; } = false;
	public int ForHome { get; set; }
	public int ForAway { get; set; }
	public string PlayerOfTheMatch { get; set; } = "";
	public string CardURL { get; set; } = "";
	public string? Postponed { get; set; }

	public bool IsPostponed => Postponed is not null;

	public string Score =>
		IsCompleted ? ForHome + " - " + ForAway : "";

	public int Id => string.IsNullOrWhiteSpace(CardURL) ? 0 :
		int.Parse(CardURL.Split('/').LastOrDefault() ?? "");
}

[DebuggerDisplay("CompletedFixture: {Date,nq} - {HomeTeam,nq} ({ForHome,nq}) vs ({ForAway,nq}) {AwayTeam,nq}")]
public class CompletedFixture : Fixture
{
}

[DebuggerDisplay("Name: {Name,nq}")]
public class Team
{
	public int Id { get; set; }
	public string URL { get; set; } = "";
	public string Caption { get; set; } = "";
	public string Name { get; set; } = "";
	public string ShortName { get; set; } = "";
	public int ClubId { get; set; }
	public string DivisionName { get; set; } = "";
	public string Captain { get; set; } = "";
	public ICollection<Player>? Players { get; set; }
	public ICollection<Fixture>? Fixtures { get; set; }
	public ICollection<Result>? Results { get; set; }
	public int? LeaguePosition {  get; set; }
	public int Played {  get; set; }
	public int Won {  get; set; }
	public int Drawn {  get; set; }
	public int Lost {  get; set; }
	public int SetsFor {  get; set; }
	public int SetsAgainst {  get; set; }
	public int Points {  get; set; }
	public int PointsAdjustment {  get; set; }


}

[DebuggerDisplay("Name: {Name,nq}")]
public class Player
{
	public string Name { get; set; } = "";
	public string PlayerURL { get; set; } = "";
	public int Played { get; set; }
	public float WinPercentage { get; set; }
	public string PoMAwards { get; set; } = "";
	public string Form { get; set; } = "";
	public int ClubRanking { get; set; }
	public int LeagueRanking { get; set; }
	public int CountyRanking { get; set; }
	public int RegionalRanking { get; set; }
	public int NationalRanking { get; set; }
	public int Id => string.IsNullOrWhiteSpace(PlayerURL) ? 0 :
		int.Parse(PlayerURL.Split('/').LastOrDefault() ?? "");
}

[DebuggerDisplay("Result: {ScoreForHome,nq} : {ScoreForAway,nq}")]
public class Result
{
	public int Id { get; set; }
	public string Opposition { get; set; } = "";
	public string HomeOrAway { get; set; } = "";
	public DateOnly Date { get; set; }
	public int ScoreForHome { get; set; }
	public int ScoreForAway { get; set; }
	public int Points { get; set; }
	public string PlayerOfTheMatch { get; set; } = "";
	public string CardURL { get; set; } = "";

	public int ScoreForTeam => HomeOrAway.ToLowerInvariant() switch
	{
		"home" => ScoreForHome,
		"away" => ScoreForAway,
		_ => throw new ArgumentOutOfRangeException(nameof(HomeOrAway))
	};

	public int ScoreForOpposition => HomeOrAway.ToLowerInvariant() switch
	{
		"home" => ScoreForAway,
		"away" => ScoreForHome,
		_ => throw new ArgumentOutOfRangeException(nameof(HomeOrAway))
	};

	public string MatchResult => (ScoreForTeam - ScoreForOpposition) switch
	{
		> 0 => "win",
		< 0 => "loss",
		0 => "draw"
	};

}

public record League(
	string Id,
	string Name,
	string Description,
	string URL,
	string Theme
	)
{
	public Season CurrentSeason { get; set; } = new("unknown", "Unknown");
	public List<Season> Seasons { get; set; } = new ();

}

public record Season(string Id, string Name)
{
	public List<Division> Divisions { get; set; } = new();
	public int DivisionCount => Divisions?.Count ?? 0;
	
}

public record Division(string Name)
{
	public List<Team> Teams { get; set; } = new ();
	public int TeamCount => Teams?.Count ?? 0;
}