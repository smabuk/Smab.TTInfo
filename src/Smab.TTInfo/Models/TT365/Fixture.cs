using System.Diagnostics;

namespace Smab.TTInfo.Models.TT365;

[DebuggerDisplay("Fixture: {Date,nq} - {HomeTeam,nq} vs {AwayTeam,nq}")]
public record Fixture
{
	public string Division { get; set; } = "";
	public string Description { get; set; } = "";
	public DateOnly Date { get; set; }
	public string HomeTeam { get; set; } = "";
	public string AwayTeam { get; set; } = "";
	public string Venue { get; set; } = "";
	public bool IsCompleted { get; set; } = false;
	public int ForHome { get; set; }
	public int ForAway { get; set; }
	public string PlayerOfTheMatch { get; set; } = "";
	public string CardURL { get; set; } = "";

	public List<MatchPlayer> HomePlayers { get; set; } = new();
	public List<MatchPlayer> AwayPlayers { get; set; } = new();


	public string Score =>
		IsCompleted ? ForHome + " - " + ForAway : "";

	public int Id => string.IsNullOrWhiteSpace(CardURL) ? 0 :
		int.Parse(CardURL.Split('/').LastOrDefault() ?? "");
}
