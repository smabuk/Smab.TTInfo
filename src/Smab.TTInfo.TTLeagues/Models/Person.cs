namespace Smab.TTInfo.TTLeagues.Models;

internal sealed record Person(
	string Id,
	string Name,
	int? MembershipNo,
	string Email,
	bool Disabled
);
