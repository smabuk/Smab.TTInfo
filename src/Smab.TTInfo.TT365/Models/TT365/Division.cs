namespace Smab.TTInfo.TT365.Models.TT365;

public record Division(string Id, string Name)
{
	public List<Team> Teams { get; set; } = [];
	public int TeamCount => Teams?.Count ?? 0;
}
