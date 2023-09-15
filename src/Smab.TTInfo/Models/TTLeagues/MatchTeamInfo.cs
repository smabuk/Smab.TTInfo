namespace Smab.TTInfo.Models.TTLeagues;

public class MatchTeamInfo
{
	public long Id { get; set; }
	public string UserId { get; set; }
	public long? TeamId { get; set; }
	public string Name { get; set; }
	public string ShortName { get; set; }
	public List<object> Members { get; set; }
	public object Score { get; set; }
	public List<object> Reserves { get; set; }
	public long Type { get; set; }
	public object Points { get; set; }
	public string Captain { get; set; }
	public long? ClubId { get; set; }
	public string CaptainId { get; set; }
	public string DisplayName { get; set; }
}
