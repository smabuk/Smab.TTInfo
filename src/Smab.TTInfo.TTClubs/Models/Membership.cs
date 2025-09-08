namespace Smab.TTInfo.TTClubs.Models;

public sealed record Membership
(
	int Page,
	int Size,
	int Total,
	int Pages,
	ImmutableList<MembershipItem> Items,
	bool HasPreviousPage,
	bool HasNextPage
);
