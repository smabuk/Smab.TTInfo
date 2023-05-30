using System.Globalization;

namespace Smab.TTInfo.Models.TT365;

[DebuggerDisplay("Name: {Name,nq}")]
public record PlayerResult
{
	public int      Id { get; set; }
	public string   Name { get; set; } = "";
	public int      OriginalSortOrder{ get; set; }
	public DateOnly Date { get; set; } = new();
	public string   PlayerTeamName { get; set; } = "";
	public Player   Opponent { get; set; } = new();
	public string   OpponentTeam { get; set; } = "";
	public string   Division{ get; set; } = "";
	public string   Scores = "";
	public int?     RankingDiff { get; set;}
	public string   Result { get; set; } = "";
	public string   ResultReason { get; set; } = "";
	public string   MatchCardURL { get; set; } = "";

	public string RankingDiffString => RankingDiff switch {
		null => "n/a",
		_    => ((int)RankingDiff).ToString("+##0;-##0", CultureInfo.CurrentCulture),
	};

	public List<Score> Games => Scores
		.Split(",")
		.Select(score => new Score(int.Parse(score[..score.IndexOf("-")]), int.Parse(score[(score.IndexOf("-")+1)..]))).ToList();

	public Score GameScore => new(Games.Count(score => score.Score1 > score.Score2), Games.Count(score => score.Score2 > score.Score1));
}
