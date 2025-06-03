namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a footer menu item for a website host.
/// </summary>
/// <param name="Id">The unique identifier for the footer item.</param>
/// <param name="ParentId">The parent identifier object.</param>
/// <param name="Parent">The parent object.</param>
/// <param name="Title">The title of the footer item.</param>
/// <param name="SubTitle">The subtitle of the footer item.</param>
/// <param name="Body">The body content of the footer item.</param>
/// <param name="Name">The name of the footer item.</param>
/// <param name="Ordering">The ordering value for display.</param>
/// <param name="Preset">The preset value.</param>
/// <param name="Content">The content object.</param>
/// <param name="Url">The URL for the footer item.</param>
/// <param name="Items">The list of items under this footer item.</param>
/// <param name="Type">The type of the footer item.</param>
/// <param name="Enabled">Indicates if the footer item is enabled.</param>
internal sealed record WebsitesHostFooter(
	int    Id,
	object ParentId,
	object Parent,
	string Title,
	string SubTitle,
	string Body,
	string Name,
	int    Ordering,
	int    Preset,
	object Content,
	string Url,
	ImmutableList<object> Items,
	int    Type,
	bool   Enabled
);

