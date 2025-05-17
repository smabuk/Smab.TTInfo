namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a tenants host in TTLeagues, including configuration and organisation details.
/// </summary>
/// <param name="Id">The unique identifier for the tenant host.</param>
/// <param name="Code">The code for the tenant host.</param>
/// <param name="Host">The host name.</param>
/// <param name="IsDefault">Indicates if this is the default tenant host.</param>
/// <param name="Name">The name of the tenant host.</param>
/// <param name="LeagueId">The league identifier.</param>
/// <param name="ClubId">The club identifier object.</param>
/// <param name="CountyId">The county identifier object.</param>
/// <param name="Updated">The last updated date.</param>
/// <param name="Created">The creation date.</param>
/// <param name="IsPublic">Indicates if the tenant host is public.</param>
/// <param name="Config">The configuration for the tenant host.</param>
/// <param name="OrganisationId">The organisation identifier.</param>
/// <param name="Organisation">The organisation details.</param>
/// <param name="IsTesting">Indicates if this is a testing tenant host.</param>
/// <param name="CheckExpiry">Indicates if expiry should be checked.</param>
/// <param name="Type">The type of tenant host.</param>
/// <param name="Deleted">The deleted object.</param>
internal sealed record TenantsHost(
	int     Id,
	string  Code,
	string  Host,
	bool    IsDefault,
	string  Name,
	int     LeagueId,
	object  ClubId,
	object  CountyId,
	DateTimeOffset Updated,
	DateTimeOffset Created,
	bool    IsPublic,
	TenantsHostConfig Config,
	int     OrganisationId,
	TenantsHostOrganisation Organisation,
	bool    IsTesting,
	bool    CheckExpiry,
	int     Type,
	object  Deleted
);
