namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents the result of a completed fixture for a specific team, including scores, points, and match outcome.
/// </summary>
/// <remarks>This record provides details about a team's performance in a completed fixture, including whether the
/// team played at home or away, the scores for both teams, and the resulting match outcome (win, loss, or
/// draw).</remarks>
[DebuggerDisplay("Result: {ScoreForHome,nq} : {ScoreForAway,nq}")]
public record TeamResult(
	string Division,
	string Description,
	DateOnly Date,
	string HomeTeam,
	string AwayTeam,
	string Venue
) : CompletedFixture(Division, Description, Date, HomeTeam, AwayTeam, Venue)
{
	public string Opposition { get; set; } = "";
	public string HomeOrAway { get; set; } = "";
	public int Points { get; set; }

	/// <summary>
	/// Gets the score for the team based on whether it is the home or away team.
	/// </summary>
	public int ScoreForTeam => HomeOrAway.ToLowerInvariant() switch
	{
		"home" => ForHome,
		"away" => ForAway,
		     _ => throw new ArgumentOutOfRangeException(nameof(HomeOrAway)),
	};

	/// <summary>
	/// Gets the score for the opposing team based on the current team's home or away status.
	/// </summary>
	public int ScoreForOpposition => HomeOrAway.ToLowerInvariant() switch
	{
		"home" => ForAway,
		"away" => ForHome,
		     _ => throw new ArgumentOutOfRangeException(nameof(HomeOrAway)),
	};

	/// <summary>
	/// Gets the result of the match based on the scores of the two teams.
	/// </summary>
	public string MatchResult => (ScoreForTeam - ScoreForOpposition) switch
	{
		> 0 => "win",
		< 0 => "loss",
		  0 => "draw",
	};
}
