namespace Smab.TTInfo.TTLeagues.Models;

internal sealed class Group
{
	public List<Match> Matches { get; set; } = new();
	public int Type { get; set; }
	public DateTimeOffset? Date { get; set; }
	public int Range { get; set; }
	public int? Week { get; set; }
	public required string Title { get; set; }
}
