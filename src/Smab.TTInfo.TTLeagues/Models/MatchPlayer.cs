namespace Smab.TTInfo.TTLeagues.Models;

internal sealed class MatchPlayer
{
	public required string UserId { get; set; }
	public required string Name { get; set; }
	public int? Score { get; set; }
}
