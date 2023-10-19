namespace Smab.TTInfo.TT365.Models.TT365;

[DebuggerDisplay("Fixture: {Date,nq} - {HomeTeam,nq} vs {AwayTeam,nq}")]
public record Fixture
{
	public string Division { get; set; } = "";
	public string Description { get; set; } = "";
	public DateOnly Date { get; set; }
	public string HomeTeam { get; set; } = "";
	public string AwayTeam { get; set; } = "";
	public string Venue { get; set; } = "";
}
