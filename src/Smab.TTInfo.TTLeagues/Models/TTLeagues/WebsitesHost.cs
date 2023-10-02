namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record WebsitesHost(
	int    TenantId,
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
	DateTimeOffset Updated,
	DateTimeOffset Created,
	IReadOnlyList<object> Images,
	IReadOnlyList<object> Links,
	WebsitesHostMenu      Menu,
	WebsitesHostConfig    Config,
	string EmailIntro,
	string EmailBody
);

