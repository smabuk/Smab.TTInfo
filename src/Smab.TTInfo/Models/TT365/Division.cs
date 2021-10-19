namespace Smab.TTInfo.Models.TT365;

public record Division(string Id, string Name)
{
	public List<Team> Teams { get; set; } = new ();
	public int TeamCount => Teams?.Count ?? 0;
}
