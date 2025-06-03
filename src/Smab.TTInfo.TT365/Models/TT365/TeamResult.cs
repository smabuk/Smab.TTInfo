namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents the result of a completed fixture for a specific team, including scores, points, and match outcome.
/// </summary>
/// <remarks>This record provides details about a team's performance in a completed fixture, including whether the
/// team played at home or away, the scores for both teams, and the resulting match outcome (win, loss, or
/// draw).</remarks>
[DebuggerDisplay("Result: {ScoreForHome,nq} : {ScoreForAway,nq}")]
public record TeamResult(
	CompletedFixture CompletedFixture,
	string Opposition,
	string HomeOrAway,
	int Points,
	bool IsVoid = false
)
{
	public DateOnly Date => CompletedFixture.Date;
	public string Division => CompletedFixture.Division;
	public string Description => CompletedFixture.Description;
	public string Venue => CompletedFixture.Venue;
	public string HomeTeam => CompletedFixture.HomeTeam;
	public string AwayTeam => CompletedFixture.AwayTeam;
	public string Score => CompletedFixture.Score;
	public int ScoreForHome => CompletedFixture.ForHome;
	public int ScoreForAway => CompletedFixture.ForAway;
	public string PlayerOfTheMatch => CompletedFixture.PlayerOfTheMatch;
	public string CardURL => CompletedFixture.CardURL;
	public int Id => CompletedFixture.Id;

	/// <summary>
	/// Gets the score for the team based on whether it is the home or away team.
	/// </summary>
	public int ScoreForTeam => HomeOrAway.ToLowerInvariant() switch
	{
		"home" => CompletedFixture.ForHome,
		"away" => CompletedFixture.ForAway,
		     _ => throw new ArgumentOutOfRangeException(nameof(HomeOrAway)),
	};

	/// <summary>
	/// Gets the score for the opposing team based on the current team's home or away status.
	/// </summary>
	public int ScoreForOpposition => HomeOrAway.ToLowerInvariant() switch
	{
		"home" => CompletedFixture.ForAway,
		"away" => CompletedFixture.ForHome,
		     _ => throw new ArgumentOutOfRangeException(nameof(HomeOrAway)),
	};

	/// <summary>
	/// Gets the result of the match based on the scores of the two teams.
	/// </summary>
	public string MatchResult => (ScoreForTeam - ScoreForOpposition) switch
	{
		_ when IsVoid is true => "void",
		> 0 => "win",
		< 0 => "loss",
		  0 => "draw",
	};
}
