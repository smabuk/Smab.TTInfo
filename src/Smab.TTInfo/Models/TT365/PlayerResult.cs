namespace Smab.TTInfo.Models.TT365;

[DebuggerDisplay("Name: {Name,nq}")]
public record PlayerResult
{
	public int Id { get; set; }
	public string Name { get; set; } = "";
	public string MatchCardURL { get; set; } = "";
	public string PlayerTeamName { get; set; } = "";
	public Player Opponent { get; set; } = new();
	public string OpponentTeam { get; set; } = "";
	public string Division{ get; set; } = "";
	public DateOnly Date { get; set; } = new();
	public string Scores = "";
	public int? RankingDiff { get; set;}
	public string Result { get; set; } = "";
	public string ResultReason { get; set; } = "";
}
