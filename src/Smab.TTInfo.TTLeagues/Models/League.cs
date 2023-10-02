namespace Smab.TTInfo.TTLeagues.Models;
internal sealed record League(
	string Id,
	TenantsHost? TenantsHost,
	WebsitesHost? WebsitesHost,
	IReadOnlyList<Competition> CurrentCompetitions,
	IReadOnlyList<Competition> ArchivedCompetitions
);
