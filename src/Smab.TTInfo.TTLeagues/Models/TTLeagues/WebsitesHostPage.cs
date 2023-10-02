namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record WebsitesHostPage(
	string Name,
	int    Ordering,
	IReadOnlyList<WebsitesHostItem> Items
);

