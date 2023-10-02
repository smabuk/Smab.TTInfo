namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
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
