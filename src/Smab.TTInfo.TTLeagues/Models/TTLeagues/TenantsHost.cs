namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
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
