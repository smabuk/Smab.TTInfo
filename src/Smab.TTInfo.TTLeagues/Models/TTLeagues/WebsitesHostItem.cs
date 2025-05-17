namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a menu item for a website host.
/// </summary>
/// <param name="Id">The unique identifier for the menu item.</param>
/// <param name="Name">The name of the menu item.</param>
/// <param name="MenuGroupId">The menu group identifier.</param>
/// <param name="Slug">The URL slug for the menu item.</param>
/// <param name="ParentId">The parent menu item identifier.</param>
/// <param name="ForwardTo">The URL or route to forward to.</param>
/// <param name="Ordering">The ordering value for display.</param>
/// <param name="Limited">Indicates if the menu item is limited in visibility.</param>
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

