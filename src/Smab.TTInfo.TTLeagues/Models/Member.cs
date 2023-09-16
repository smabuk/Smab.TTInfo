namespace Smab.TTInfo.TTLeagues.Models;
public class Member
{
	public int? TeamId { get; set; }
	public required string MemberId { get; set; }
	public required string Name { get; set; }
	public int? Ordering { get; set; }
	public bool? Playing { get; set; }
	public int? TotalStepUp { get; set; }
	public object? ImageId { get; set; }
	public required string Team { get; set; }
	public bool? Disabled { get; set; }
}
