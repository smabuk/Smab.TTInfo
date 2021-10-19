using System.Diagnostics;

namespace Smab.TTInfo.Models.TT365;

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
