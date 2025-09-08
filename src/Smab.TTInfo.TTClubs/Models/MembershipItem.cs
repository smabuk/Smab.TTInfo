namespace Smab.TTInfo.TTClubs.Models;

public sealed record MembershipItem
(
	int Id,
	int ContactId,
	Contact Contact,
	int MembershipId,
	object MembershipNo,
	object ParentId,
	object Parent,
	DateTime Joined,
	DateTime Start,
	DateTime End,
	DateTime? Confirmed,
	int Usage,
	int Status,
	float Cost,
	float SetupFee,
	DateTime? Due,
	object AgreedTerms,
	bool Approved,
	bool Upfront,
	bool AutoRenew
);
