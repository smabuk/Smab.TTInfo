namespace Smab.TTInfo.TT365.Models.TT365;

//[DebuggerDisplay("Name: {Name,nq}")]
public record MatchSet(
	int SetNo,
	Player HomePlayer,
	Player AwayPlayer,
	string Scores,
	string Result,
	string ResultReason
	)
{
	public Player? HomeDoublesPartner { get; set; }
	public Player? AwayDoublesPartner { get; set; }


	/// <summary>
	/// Gets the list of games, where each game is represented by a score.
	/// </summary>
	public List<Score> Games { get; init; } = Scores.Contains('-')
		? [.. Scores
		.Split(",")
		.Select(score => new Score(int.Parse(score[..score.IndexOf('-')]), int.Parse(score[(score.IndexOf('-') + 1)..])))]
		: [];

	/// <summary>
	/// Gets the current game score, represented as a <see cref="Score"/> object.
	/// </summary>
	public Score GameScore => new(Games.Count(score => score.Score1 > score.Score2), Games.Count(score => score.Score2 > score.Score1));
}
