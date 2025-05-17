namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a competition configuration for a tenant, including submission and approval types, ranking settings, and
/// restrictions.
/// </summary>
/// <param name="TenantId">The unique identifier of the tenant hosting the competition.</param>
/// <param name="SubmissionType">The type of submission allowed for the competition. Valid values depend on the competition's rules.</param>
/// <param name="ApprovalType">The type of approval process required for submissions. Valid values depend on the competition's rules.</param>
/// <param name="RankingEnabled">A value indicating whether ranking is enabled for the competition. <see langword="true"/> if ranking is enabled;
/// otherwise, <see langword="false"/>.</param>
/// <param name="GlobalMembers">A value indicating whether the competition is open to global members. <see langword="true"/> if global members are
/// allowed; otherwise, <see langword="false"/>.</param>
/// <param name="RestrictByClub">A value indicating whether participation is restricted by club membership. <see langword="true"/> if restricted;
/// otherwise, <see langword="false"/>.</param>
/// <param name="RestrictByMembership">A string specifying the membership type required to participate in the competition. This can be an empty string if
/// no specific membership restriction applies.</param>
internal sealed record TenantsHostCompetition(
	int    TenantId,
	int    SubmissionType,
	int    ApprovalType,
	bool   RankingEnabled,
	bool   GlobalMembers,
	bool   RestrictByClub,
	string RestrictByMembership
);
