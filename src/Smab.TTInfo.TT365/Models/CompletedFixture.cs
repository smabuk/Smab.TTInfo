namespace Smab.TTInfo.TT365.Models;

[DebuggerDisplay("CompletedFixture: {Date,nq} - {HomeTeam,nq} ({ForHome,nq}) vs ({ForAway,nq}) {AwayTeam,nq}")]
public record CompletedFixture : Fixture
{
	public int ForHome { get; set; }
	public int ForAway { get; set; }
	public string PlayerOfTheMatch { get; set; } = "";
	public string CardURL { get; set; } = "";
	public List<MatchPlayer> HomePlayers { get; set; } = [];
	public List<MatchPlayer> AwayPlayers { get; set; } = [];
	public string? Other { get; set; }
	public int Id => string.IsNullOrWhiteSpace(CardURL) ? 0 :
		int.Parse(CardURL.Split('/').LastOrDefault() ?? "");
	public string DoublesWinner => ForHome - HomePlayers.Sum(p => p.SetsWon) > 0 ? HomeTeam : AwayTeam;

	public string Score => $"{ForHome} - {ForAway}";
}
