namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

public sealed record Person(
	string  Id,
	string  Name,
	int?    MembershipNo,
	string  Email,
	bool    Disabled
);
