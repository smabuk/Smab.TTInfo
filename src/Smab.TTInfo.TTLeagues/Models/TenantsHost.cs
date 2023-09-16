﻿namespace Smab.TTInfo.TTLeagues.Models;
public record TenantsHostBooking(
	int TenantId,
	bool CompleteProfileRequired,
	int BookingTimeFrame);

public record TenantsHostCompetition(
	int TenantId,
	int SubmissionType,
	int ApprovalType,
	bool RankingEnabled,
	bool GlobalMembers,
	bool RestrictByClub,
	string RestrictByMembership);

public record TenantsHostConfig(
	int TenantId,
	bool AppEnabled,
	string StripeId,
	bool ChargesEnabled,
	bool CompetitionsModule,
	bool BookingsModule,
	bool FastFormatModule,
	bool FastOnly,
	bool AllowNationalRankings,
	bool AutoCharge,
	bool AutoRenew,
	bool AllowGlobalSearch,
	bool AllowAdvancedOptions,
	bool AllowTeamChecker,
	bool RegistrationsModule,
	TenantsHostCompetition Competition,
	TenantsHostBooking Booking);

public record TenantsHostOrganisation(
	int Id,
	string Name,
	string Logo,
	int Import,
	string Logins,
	List<int> LoginModes,
	bool RegistrationEnabled,
	bool ReportingEnabled,
	bool CompeteMembershipRequired,
	string Website,
	string News,
	string Privacy,
	string Terms,
	string Twitter,
	string Facebook,
	string Instagram);

public record TenantsHost(
	int Id,
	string Code,
	string Host,
	bool IsDefault,
	string Name,
	int LeagueId,
	object ClubId,
	object CountyId,
	DateTime Updated,
	DateTime Created,
	bool IsPublic,
	TenantsHostConfig Config,
	int OrganisationId,
	TenantsHostOrganisation Organisation,
	bool IsTesting,
	bool CheckExpiry,
	int Type,
	object Deleted);
