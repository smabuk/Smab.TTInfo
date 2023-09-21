namespace Smab.TTInfo.TTLeagues.Models;
internal sealed record WebsitesHostArchived(
	string Id,
	string Name,
	string MenuGroupId,
	string Slug,
	string ParentId,
	string ForwardTo,
	int Ordering,
	bool Limited
);

internal sealed record WebsitesHostCompetition(
	string Id,
	string Name,
	string MenuGroupId,
	string Slug,
	string ParentId,
	string ForwardTo,
	int Ordering,
	bool Limited
);

internal sealed record WebsitesHostConfig(
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

internal sealed record WebsitesHostDivision(
	string Id,
	string Name,
	string MenuGroupId,
	string Slug,
	string ParentId,
	string ForwardTo,
	int Ordering,
	bool Limited
);

internal sealed record WebsitesHostFeed(
	string Id,
	string Name,
	string MenuGroupId,
	string Slug,
	string ParentId,
	string ForwardTo,
	int Ordering,
	bool Limited
);

internal sealed record WebsitesHostFooter(
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

internal sealed record WebsitesHostItem(
	string Id,
	string Name,
	string MenuGroupId,
	string Slug,
	string ParentId,
	string ForwardTo,
	int Ordering,
	bool Limited
);

internal sealed record WebsitesHostMenu(
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

internal sealed record WebsitesHostNews(
	string Id,
	string Name,
	string MenuGroupId,
	string Slug,
	string ParentId,
	string ForwardTo,
	int Ordering,
	bool Limited
);

internal sealed record WebsitesHostPage(
	string Name,
	int Ordering,
	IReadOnlyList<WebsitesHostItem> Items
);

internal sealed record WebsitesHostPrimary(
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

internal sealed record WebsitesHost(
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

