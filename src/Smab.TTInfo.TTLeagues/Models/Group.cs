namespace Smab.TTInfo.TTLeagues.Models;

internal sealed class Group
{
	public List<Match> Matches { get; set; } = new();
	public long Type { get; set; }
	public DateTime Date { get; set; }
	public long Range { get; set; }
	public long Week { get; set; }
	public required string Title { get; set; }
}
