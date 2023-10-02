namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
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
