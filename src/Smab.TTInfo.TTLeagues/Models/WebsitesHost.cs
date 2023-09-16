namespace Smab.TTInfo.TTLeagues.Models;
public record WebsitesHostArchived(
	string Id,
	string Name,
	string MenuGroupId,
	string Slug,
	string ParentId,
	string ForwardTo,
	int Ordering,
	bool Limited
);

public record WebsitesHostCompetition(
	string Id,
	string Name,
	string MenuGroupId,
	string Slug,
	string ParentId,
	string ForwardTo,
	int Ordering,
	bool Limited
);

public record WebsitesHostConfig(
	string ReplyEmail,
	string FromEmail,
	bool HomeHeader,
	bool SubHeader,
	bool ResultsCarousel,
	bool AppEnabled,
	int SubmissionType,
	int ApprovalType,
	int CompetitionMenuDisplay,
	int Visibility,
	string RedirectUrl,
	bool AutoScrollCheck,
	int CompetitionPreference,
	int MaxArticles,
	object Published
);

public record WebsitesHostDivision(
	string Id,
	string Name,
	string MenuGroupId,
	string Slug,
	string ParentId,
	string ForwardTo,
	int Ordering,
	bool Limited
);

public record WebsitesHostFeed(
	string Id,
	string Name,
	string MenuGroupId,
	string Slug,
	string ParentId,
	string ForwardTo,
	int Ordering,
	bool Limited
);

public record WebsitesHostFooter(
	int Id,
	object ParentId,
	object Parent,
	string Title,
	string SubTitle,
	string Body,
	string Name,
	int Ordering,
	int Preset,
	object Content,
	string Url,
	IReadOnlyList<object> Items,
	int Type,
	bool Enabled
);

public record WebsitesHostItem(
	string Id,
	string Name,
	string MenuGroupId,
	string Slug,
	string ParentId,
	string ForwardTo,
	int Ordering,
	bool Limited
);

public record WebsitesHostMenu(
	IReadOnlyList<WebsitesHostCompetition> Competitions,
	IReadOnlyList<WebsitesHostDivision> Divisions,
	IReadOnlyList<WebsitesHostArchived> Archived,
	IReadOnlyList<WebsitesHostNews> News,
	IReadOnlyList<WebsitesHostPage> Pages,
	IReadOnlyList<WebsitesHostFeed> Feed,
	IReadOnlyList<WebsitesHostPrimary> Primary,
	IReadOnlyList<object> Secondary,
	IReadOnlyList<WebsitesHostFooter> Footer
);

public record WebsitesHostNews(
	string Id,
	string Name,
	string MenuGroupId,
	string Slug,
	string ParentId,
	string ForwardTo,
	int Ordering,
	bool Limited
);

public record WebsitesHostPage(
	string Name,
	int Ordering,
	IReadOnlyList<WebsitesHostItem> Items
);

public record WebsitesHostPrimary(
	int Id,
	object ParentId,
	object Parent,
	string Title,
	string SubTitle,
	string Body,
	string Name,
	int Ordering,
	int Preset,
	object Content,
	string Url,
	IReadOnlyList<object> Items,
	int Type,
	bool Enabled
);

public record WebsitesHost(
	int TenantId,
	string Title,
	string Description,
	string About,
	string Contact,
	string Instagram,
	string Twitter,
	string Facebook,
	string WhatsApp,
	string Email,
	string Mobile,
	string UpdatedById,
	DateTime Updated,
	DateTime Created,
	IReadOnlyList<object> Images,
	IReadOnlyList<object> Links,
	WebsitesHostMenu Menu,
	WebsitesHostConfig Config,
	string EmailIntro,
	string EmailBody
);

