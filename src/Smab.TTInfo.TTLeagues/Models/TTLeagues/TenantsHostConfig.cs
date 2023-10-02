namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
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
