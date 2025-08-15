namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a member of a team with associated details such as name, membership number, and status.
/// </summary>
/// <remarks>This record is used to encapsulate information about a team member, including their unique
/// identifiers, display name, membership status, and other optional attributes. It is immutable and designed for
/// scenarios where team member data needs to be passed or stored in a structured format.</remarks>
/// <param name="TeamId"></param>
/// <param name="MemberId"></param>
/// <param name="Name"></param>
/// <param name="Ordering"></param>
/// <param name="Initials"></param>
/// <param name="MembershipNo"></param>
/// <param name="Expired"></param>
/// <param name="Associated"></param>
public sealed record TeamMember(
	int?    TeamId,
	string  MemberId,
	string  Name,
	int?    Ordering,
	string  Initials,
	int?    MembershipNo,
	bool?   Expired,
	bool?   Associated
);
