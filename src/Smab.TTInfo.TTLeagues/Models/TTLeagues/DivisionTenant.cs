namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a tenant within a division, containing information about its configuration, associated organization, and
/// other metadata.
/// </summary>
/// <remarks>This record encapsulates details about a division tenant, including its unique identifier, associated
/// league and organization, configuration settings, and timestamps for creation and updates. It also includes flags for
/// default status, public visibility, and testing mode.</remarks>
/// <param name="Id"></param>
/// <param name="Code"></param>
/// <param name="Host"></param>
/// <param name="IsDefault"></param>
/// <param name="Name"></param>
/// <param name="LeagueId"></param>
/// <param name="ClubId"></param>
/// <param name="CountyId"></param>
/// <param name="Updated"></param>
/// <param name="Created"></param>
/// <param name="IsPublic"></param>
/// <param name="Config"></param>
/// <param name="OrganisationId"></param>
/// <param name="Organisation"></param>
/// <param name="IsTesting"></param>
/// <param name="CheckExpiry"></param>
/// <param name="Type"></param>
/// <param name="Deleted"></param>
internal sealed record DivisionTenant(
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
	DivisionConfig Config,
	int     OrganisationId,
	DivisionOrganisation Organisation,
	bool    IsTesting,
	bool    CheckExpiry,
	int     Type,
	object  Deleted
);
