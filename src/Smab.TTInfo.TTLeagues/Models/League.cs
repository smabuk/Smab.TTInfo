using Smab.TTInfo.TTLeagues.Models.TTLeagues;

namespace Smab.TTInfo.TTLeagues.Models;

/// <summary>
/// Represents a TTLeagues league, including host, competitions, and identifiers.
/// </summary>
/// <param name="TTInfoId">The TTInfo identifier for the league.</param>
/// <param name="TenantsHost">The tenants host information.</param>
/// <param name="WebsitesHost">The websites host information.</param>
/// <param name="CurrentCompetitions">The list of current competitions.</param>
/// <param name="ArchivedCompetitions">The list of archived competitions.</param>
internal sealed record League(
	string TTInfoId,
	TenantsHost? TenantsHost,
	WebsitesHost? WebsitesHost,
	ImmutableList<Competition> CurrentCompetitions,
	ImmutableList<Competition> ArchivedCompetitions
);
