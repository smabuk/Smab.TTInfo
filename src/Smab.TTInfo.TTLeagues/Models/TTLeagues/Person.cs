namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a person in TTLeagues, such as an organiser or player.
/// </summary>
/// <param name="Id">The unique identifier for the person.</param>
/// <param name="Name">The name of the person.</param>
/// <param name="MembershipNo">The membership number, if available.</param>
/// <param name="Email">The email address of the person.</param>
/// <param name="Disabled">Indicates whether the person is disabled.</param>
public sealed record Person(
	string Id,
	string Name,
	int?   MembershipNo,
	string Email,
	bool   Disabled
);
