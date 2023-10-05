namespace Smab.TTInfo.TTLeagues.Models;
internal sealed record League(
	string TTInfoId,
	TenantsHost? TenantsHost,
	WebsitesHost? WebsitesHost,
	IReadOnlyList<Competition> CurrentCompetitions,
	IReadOnlyList<Competition> ArchivedCompetitions
);
