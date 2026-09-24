namespace Smab.TTInfo.TT365.Models.TT365;

//[DebuggerDisplay("Name: {Name,nq}")]
public sealed record MatchSet(
	int SetNo,
	Player HomePlayer,
	Player AwayPlayer,
	string Scores,
	string Result,
	string? ResultReason
	)
{
	public Player? HomeDoublesPartner { get; set; }
	public Player? AwayDoublesPartner { get; set; }


	/// <summary>
	/// Gets the list of games, where each game is represented by a score.
	/// </summary>
	public List<Score> Games { get; init; } = Scores.Contains('-')
		? [.. Scores
		.Split(",", StringSplitOptions.TrimEntries)
		.Select(score => new Score(int.Parse(score[..score.IndexOf('-')]), int.Parse(score[(score.IndexOf('-') + 1)..])))]
		: [];

	/// <summary>
	/// Gets the current game score, represented as a <see cref="Score"/> object.
	/// </summary>
	public Score GameScore => new(Games.Count(score => score.Score1 > score.Score2), Games.Count(score => score.Score2 > score.Score1));

	public List<Player> HomePlayers => HomeDoublesPartner is not null ? [HomePlayer, HomeDoublesPartner] : [HomePlayer];
	public List<Player> AwayPlayers => AwayDoublesPartner is not null ? [AwayPlayer, AwayDoublesPartner] : [AwayPlayer];

	public int HomeScore => GameScore.Score1;
	public int AwayScore => GameScore.Score2;

	public bool IsDoubles => HomeDoublesPartner is not null || AwayDoublesPartner is not null;
	public bool HomeWin => GameScore.Score1 > GameScore.Score2;
	public bool AwayWin => GameScore.Score2 > GameScore.Score1;
}
