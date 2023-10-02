namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record TenantsHostCompetition(
	int    TenantId,
	int    SubmissionType,
	int    ApprovalType,
	bool   RankingEnabled,
	bool   GlobalMembers,
	bool   RestrictByClub,
	string RestrictByMembership
);
