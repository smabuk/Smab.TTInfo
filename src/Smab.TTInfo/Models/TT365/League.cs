namespace Smab.TTInfo.Models.TT365;

public record League(
	string Id,
	string Name,
	string Description,
	string URL,
	string Theme
	)
{
	public Season CurrentSeason { get; set; } = new("unknown", "Unknown");
	public List<Season> Seasons { get; set; } = new();

}
