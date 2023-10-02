namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record WebsitesHostItem(
	string Id,
	string Name,
	string MenuGroupId,
	string Slug,
	string ParentId,
	string ForwardTo,
	int    Ordering,
	bool   Limited
);

