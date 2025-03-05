using System.Globalization;

namespace Smab.TTInfo.TT365.Models.TT365;

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
	public string FormattedRankingDiff { get; init; } = RankingDiff?.ToString("+##0;-##0;0", CultureInfo.CurrentCulture) ?? "n/a";

	public List<Score> Games { get; init; } = [.. Scores
		.Split(",")
		.Select(score => new Score(int.Parse(score[..score.IndexOf("-")]), int.Parse(score[(score.IndexOf("-") + 1)..])))];

	public Score GameScore => new(Games.Count(score => score.Score1 > score.Score2), Games.Count(score => score.Score2 > score.Score1));
}
