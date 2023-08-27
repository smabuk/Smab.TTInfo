namespace Smab.TTInfo.Models.TT365;

[DebuggerDisplay("Name: {Name,nq}")]
public class Team
{
	public string Id { get; set; } = "";
	public string URL { get; set; } = "";
	public string Caption { get; set; } = "";
	public string Name { get; set; } = "";
	public string ShortName { get; set; } = "";
	public int ClubId { get; set; }
	public string DivisionName { get; set; } = "";
	public string Captain { get; set; } = "";
	public ICollection<Player>? Players { get; set; }
	public ICollection<Fixture>? Fixtures { get; set; }
	public ICollection<TeamResult>? Results { get; set; }
	public int? LeaguePosition {  get; set; }
	public int Played {  get; set; }
	public int Won {  get; set; }
	public int Drawn {  get; set; }
	public int Lost {  get; set; }
	public int SetsFor {  get; set; }
	public int SetsAgainst {  get; set; }
	public int Points {  get; set; }
	public int PointsAdjustment {  get; set; }
}
