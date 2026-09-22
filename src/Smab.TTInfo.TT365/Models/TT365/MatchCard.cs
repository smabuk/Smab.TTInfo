namespace Smab.TTInfo.TT365.Models.TT365;

public record MatchCard(
	int Id,
	DateOnly Date,
	string HomeTeam,
	string AwayTeam,
	string DivisionName,
	double ForHome,
	double ForAway)
{
	public string Description { get; set; } = "";
	public string Venue { get; set; } = "";
	public string PlayerOfTheMatch { get; set; } = "";
	public string CardURL { get; set; } = "";
	public List<MatchPlayer> HomePlayers { get; set; } = [];
	public List<MatchPlayer> AwayPlayers { get; set; } = [];
	public List<MatchSet> Sets { get; set; } = [];
};
