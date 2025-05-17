namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a page in a website host menu.
/// </summary>
/// <param name="Name">The name of the page.</param>
/// <param name="Ordering">The ordering value for the page.</param>
/// <param name="Items">The list of menu items on the page.</param>
internal sealed record WebsitesHostPage(
	string Name,
	int    Ordering,
	IReadOnlyList<WebsitesHostItem> Items
);

