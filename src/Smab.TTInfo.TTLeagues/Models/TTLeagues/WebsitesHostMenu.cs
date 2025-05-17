namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a menu for a website host, including competitions, divisions, news, and more.
/// </summary>
/// <param name="Competitions">List of competition menu items.</param>
/// <param name="Divisions">List of division menu items.</param>
/// <param name="Archived">List of archived menu items.</param>
/// <param name="News">List of news menu items.</param>
/// <param name="Pages">List of website pages.</param>
/// <param name="Feed">List of feed menu items.</param>
/// <param name="Primary">List of primary menu items.</param>
/// <param name="Secondary">List of secondary menu items.</param>
/// <param name="Footer">List of footer menu items.</param>
internal sealed record WebsitesHostMenu(
	IReadOnlyList<WebsitesHostItem>    Competitions,
	IReadOnlyList<WebsitesHostItem>    Divisions,
	IReadOnlyList<WebsitesHostItem>    Archived,
	IReadOnlyList<WebsitesHostItem>    News,
	IReadOnlyList<WebsitesHostPage>    Pages,
	IReadOnlyList<WebsitesHostItem>    Feed,
	IReadOnlyList<WebsitesHostPrimary> Primary,
	IReadOnlyList<object>              Secondary,
	IReadOnlyList<WebsitesHostFooter>  Footer
);
