namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents the configuration settings for a tenant in the hosting system.
/// </summary>
/// <remarks>This record encapsulates various configuration options and modules enabled for a specific tenant,
/// including payment settings, module availability, and feature flags. It is used to manage tenant-specific behavior
/// and capabilities within the application.</remarks>
/// <param name="TenantId"></param>
/// <param name="AppEnabled"></param>
/// <param name="StripeId"></param>
/// <param name="ChargesEnabled"></param>
/// <param name="CompetitionsModule"></param>
/// <param name="BookingsModule"></param>
/// <param name="FastFormatModule"></param>
/// <param name="FastOnly"></param>
/// <param name="AllowNationalRankings"></param>
/// <param name="AutoCharge"></param>
/// <param name="AutoRenew"></param>
/// <param name="AllowGlobalSearch"></param>
/// <param name="AllowAdvancedOptions"></param>
/// <param name="AllowTeamChecker"></param>
/// <param name="RegistrationsModule"></param>
/// <param name="Competition"></param>
/// <param name="Booking"></param>
internal sealed record TenantsHostConfig(
	int    TenantId,
	bool   AppEnabled,
	string StripeId,
	bool   ChargesEnabled,
	bool   CompetitionsModule,
	bool   BookingsModule,
	bool   FastFormatModule,
	bool   FastOnly,
	bool   AllowNationalRankings,
	bool   AutoCharge,
	bool   AutoRenew,
	bool   AllowGlobalSearch,
	bool   AllowAdvancedOptions,
	bool   AllowTeamChecker,
	bool   RegistrationsModule,
	TenantsHostCompetition Competition,
	TenantsHostBooking     Booking
);
