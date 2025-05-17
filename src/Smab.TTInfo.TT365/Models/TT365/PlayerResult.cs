using System.Globalization;

namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents the result of a player's match, including details about the player, opponent, scores, and match outcome.
/// </summary>
/// <remarks>This record encapsulates information about a player's match result, including the player's team,
/// opponent details, scores for individual games, and the overall match result. It also provides derived properties for
/// formatted ranking differences and aggregated game scores.</remarks>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="OriginalSortOrder"></param>
/// <param name="Date"></param>
/// <param name="PlayerTeamName"></param>
/// <param name="Opponent"></param>
/// <param name="OpponentTeam"></param>
/// <param name="Division"></param>
/// <param name="Scores"></param>
/// <param name="RankingDiff"></param>
/// <param name="Result"></param>
/// <param name="ResultReason"></param>
/// <param name="MatchCardURL"></param>
[DebuggerDisplay("Name: {Name,nq}")]
public record PlayerResult(
	int Id,
	string Name,
	int OriginalSortOrder,
	DateOnly Date,
	string PlayerTeamName,
	Player Opponent,
	string OpponentTeam,
	string Division,
	string Scores,
	int? RankingDiff,
	string Result,
	string ResultReason,
	string MatchCardURL
	)
{
	/// <summary>
	/// Gets the formatted ranking difference as a string.
	/// </summary>
	public string FormattedRankingDiff { get; init; } = RankingDiff?.ToString("+##0;-##0;0", CultureInfo.CurrentCulture) ?? "n/a";

	/// <summary>
	/// Gets the list of games, where each game is represented by a score.
	/// </summary>
	public List<Score> Games { get; init; } = [.. Scores
		.Split(",")
		.Select(score => new Score(int.Parse(score[..score.IndexOf('-')]), int.Parse(score[(score.IndexOf('-') + 1)..])))];

	/// <summary>
	/// Gets the current game score, represented as a <see cref="Score"/> object.
	/// </summary>
	public Score GameScore => new(Games.Count(score => score.Score1 > score.Score2), Games.Count(score => score.Score2 > score.Score1));
}
