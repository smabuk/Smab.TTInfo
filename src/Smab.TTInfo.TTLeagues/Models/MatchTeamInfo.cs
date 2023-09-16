namespace Smab.TTInfo.TTLeagues.Models;

public class MatchTeamInfo
{
	public long Id { get; set; }
	public required string UserId { get; set; }
	public long? TeamId { get; set; }
	public required string Name { get; set; }
	public required string ShortName { get; set; }
	public List<Member>? Members { get; set; }
	public long? Score { get; set; }
	public List<object>? Reserves { get; set; }
	public long Type { get; set; }
	public long? Points { get; set; }
	public required string Captain { get; set; }
	public long? ClubId { get; set; }
	public required string CaptainId { get; set; }
	public required string DisplayName { get; set; }
}
