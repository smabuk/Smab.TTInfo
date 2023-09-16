namespace Smab.TTInfo.Models.TTLeagues;

public partial class MatchPlayer
{
	public required string UserId { get; set; }
	public required string Name { get; set; }
	public long? Score { get; set; }
}
