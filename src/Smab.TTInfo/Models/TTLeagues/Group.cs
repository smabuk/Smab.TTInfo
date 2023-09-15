namespace Smab.TTInfo.Models.TTLeagues;

public partial class Group
{
	public List<Match> Matches { get; set; } = new();
	public long Type { get; set; }
	public DateTimeOffset Date { get; set; }
	public long Range { get; set; }
	public long Week { get; set; }
	public required string Title { get; set; }
}
