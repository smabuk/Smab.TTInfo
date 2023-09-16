namespace Smab.TTInfo.Models.TTLeagues;

public class ActionedBy
{
	public required string Id { get; set; }
	public required string Name { get; set; }
	public object? MembershipNo { get; set; }
	public required string Email { get; set; }
	public bool Disabled { get; set; }
}
