namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a member of a team with associated details such as their ID, name, status, and other attributes.
/// </summary>
/// <remarks>This record is used to encapsulate information about a team member, including their team association,
/// playing status, and other optional metadata. It is immutable and designed for scenarios where member details need to
/// be passed or stored in a structured format.</remarks>
/// <param name="TeamId"></param>
/// <param name="MemberId"></param>
/// <param name="Name"></param>
/// <param name="Ordering"></param>
/// <param name="Playing"></param>
/// <param name="TotalStepUp"></param>
/// <param name="ImageId"></param>
/// <param name="Team"></param>
/// <param name="Disabled"></param>
internal sealed record Member(
	int?    TeamId,
	string  MemberId,
	string  Name,
	int?    Ordering,
	bool?   Playing,
	int?    TotalStepUp,
	object? ImageId,
	string  Team,
	bool?   Disabled
);
