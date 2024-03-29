﻿namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record TeamMember(
	int?    TeamId,
	string  MemberId,
	string  Name,
	int?    Ordering,
	string  Initials,
	int?    MembershipNo,
	bool?   Expired,
	bool?   Associated
);
