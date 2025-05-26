namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents a completed fixture in a match, including the final score, participating players, and other details.
/// </summary>
/// <remarks>This record extends the <see cref="Fixture"/> type to include additional information specific to
/// completed matches,  such as the final score, the player of the match, and details about the players who
/// participated.</remarks>
[DebuggerDisplay("CompletedFixture: {Date,nq} - {HomeTeam,nq} ({ForHome,nq}) vs ({ForAway,nq}) {AwayTeam,nq}")]
public record CompletedFixture(
	string Division,
	string Description,
	DateOnly Date,
	string HomeTeam,
	string AwayTeam,
	string Venue
) : Fixture(Division, Description, Date, HomeTeam, AwayTeam, Venue)
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
