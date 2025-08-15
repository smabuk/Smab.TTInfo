namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a primary menu item for a website host.
/// </summary>
/// <param name="Id">The unique identifier for the primary menu item.</param>
/// <param name="ParentId">The parent identifier object.</param>
/// <param name="Parent">The parent object.</param>
/// <param name="Title">The title of the primary menu item.</param>
/// <param name="SubTitle">The subtitle of the primary menu item.</param>
/// <param name="Body">The body content of the primary menu item.</param>
/// <param name="Name">The name of the primary menu item.</param>
/// <param name="Ordering">The ordering value for display.</param>
/// <param name="Preset">The preset value.</param>
/// <param name="Content">The content object.</param>
/// <param name="Url">The URL for the menu item.</param>
/// <param name="Items">The list of items under this primary menu item.</param>
/// <param name="Type">The type of the menu item.</param>
/// <param name="Enabled">Indicates if the menu item is enabled.</param>
public sealed record WebsitesHostPrimary(
	int     Id,
	object  ParentId,
	object  Parent,
	string  Title,
	string  SubTitle,
	string  Body,
	string  Name,
	int     Ordering,
	int     Preset,
	object  Content,
	string  Url,
	ImmutableList<object> Items,
	int     Type,
	bool    Enabled
);
