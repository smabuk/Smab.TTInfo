namespace Smab.TTInfo.TT365.Models.TT365;

[DebuggerDisplay("Result: {ScoreForHome,nq} : {ScoreForAway,nq}")]
public record TeamResult : CompletedFixture
{
	public string Opposition { get; set; } = "";
	public string HomeOrAway { get; set; } = "";
	public int Points { get; set; }

	public int ScoreForTeam => HomeOrAway.ToLowerInvariant() switch
	{
		"home" => ForHome,
		"away" => ForAway,
		_ => throw new ArgumentOutOfRangeException(nameof(HomeOrAway)),
	};

	public int ScoreForOpposition => HomeOrAway.ToLowerInvariant() switch
	{
		"home" => ForAway,
		"away" => ForHome,
		_ => throw new ArgumentOutOfRangeException(nameof(HomeOrAway)),
	};

	public string MatchResult => (ScoreForTeam - ScoreForOpposition) switch
	{
		> 0 => "win",
		< 0 => "loss",
		0 => "draw",
	};
}
