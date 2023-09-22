namespace Smab.TTInfo.TT365.Models;

[DebuggerDisplay("Name: {Name,nq}")]
public class Player
{
	public string Name { get; set; } = "";
	public string PlayerURL { get; set; } = "";
	public int Played { get; set; }
	public float WinPercentage { get; set; }
	public string PoMAwards { get; set; } = "";
	public string Form { get; set; } = "";
	public int ClubRanking { get; set; }
	public int LeagueRanking { get; set; }
	public int CountyRanking { get; set; }
	public int RegionalRanking { get; set; }
	public int NationalRanking { get; set; }
	public int PlayerId { get; set; }
	public int Id => PlayerId == 0
		? string.IsNullOrWhiteSpace(PlayerURL)
			? 0
			: int.Parse(PlayerURL.Split('/').LastOrDefault() ?? "")
		: PlayerId;
	public string SeasonId => PlayerURL.Split('/').Skip(4).FirstOrDefault() ?? "";

	public List<PlayerResult> PlayerResults { get; set; } = new();
}
