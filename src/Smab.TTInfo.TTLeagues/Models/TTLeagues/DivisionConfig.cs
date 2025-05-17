namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents the configuration settings for a division, including feature flags, payment settings, and module
/// availability.
/// </summary>
/// <remarks>This record encapsulates various configuration options for a division, such as enabled modules,
/// payment capabilities,  and competition or booking-specific settings. It is intended to be used as a centralized
/// representation of a division's  operational configuration.</remarks>
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
internal sealed record DivisionConfig(
	int     TenantId,
	bool    AppEnabled,
	string  StripeId,
	bool    ChargesEnabled,
	bool    CompetitionsModule,
	bool    BookingsModule,
	bool    FastFormatModule,
	bool    FastOnly,
	bool    AllowNationalRankings,
	bool    AutoCharge,
	bool    AutoRenew,
	bool    AllowGlobalSearch,
	bool    AllowAdvancedOptions,
	bool    AllowTeamChecker,
	bool    RegistrationsModule,
	DivisionCompetition Competition,
	DivisionBooking     Booking
);
