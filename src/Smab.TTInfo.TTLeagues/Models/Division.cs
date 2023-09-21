namespace Smab.TTInfo.TTLeagues.Models;
internal sealed record Division(
	int Id,
	string Name,
	string Description,
	int CompetitionId,
	DivisionCompetition Competition,
	string UserId,
	int Ordering,
	object MiniOrdering,
	int Status,
	string Details,
	DateTime Updated,
	object StartDate,
	object Reserves,
	object GamesPerSet,
	object GameFormat,
	object FormatId,
	object Format,
	object PointId,
	object Point,
	object FixtureFormat,
	int? PlayEachOther,
	object Deleted,
	object BaseRank,
	object PlayAll,
	object SplitRounds,
	object MiniStartingRound,
	bool MiniDivision,
	object PreviousLinkId,
	object PreviousLink,
	object HalfGeneration,
	string TableNos,
	bool TableAllocation,
	bool HasEntrants,
	int EntrantCount,
	IReadOnlyList<object> Days
)
{
	// Added manually - not part of API call
	public List<TeamStanding> TeamStandings { get; set; } = [];
};

internal sealed record DivisionAverages(
	int CompetitionId,
	bool SingleForfeit,
	bool SingleConcede,
	bool SingleWalkover,
	bool DoublesForfeit,
	bool DoublesConcede,
	bool DoublesWalkover,
	bool ExcludeReserves,
	int CalculationType,
	int OrderType,
	bool ExcludeNoWins,
	int MinimumPlayed,
	bool ConsolidatedAverages
);

internal sealed record DivisionBooking(
	int TenantId,
	bool CompleteProfileRequired,
	int BookingTimeFrame
);

internal sealed record DivisionCompetition(
	int Id,
	int OldLeagueId,
	object OldLeague,
	string Name,
	string MenuName,
	DateTime StartDate,
	string UserId,
	int FormatId,
	object Format,
	int GamesPerSet,
	int GameFormat,
	string Description,
	string Details,
	int PointId,
	object MinAge,
	object MaxAge,
	DivisionMatchCard MatchCard,
	DivisionTable Table,
	DivisionAverages Averages,
	DivisionReserves Reserves,
	object EndDate,
	string OrganiserId,
	object Organiser,
	bool PlayAll,
	int PlayEachOther,
	object Deleted,
	DateTime Ready,
	DateTime Published,
	object StartTime,
	object Archived,
	object Hidden,
	int FixtureFormat,
	int ResultDisplay,
	object Night,
	object TableCount,
	bool ManualScoring,
	int Season,
	bool Ranking,
	int TenantId,
	DivisionTenant Tenant,
	object GroupId,
	object Group,
	object GroupOrdering,
	bool SplitRounds,
	int CaptainType,
	int PlayerType,
	bool HalfGeneration,
	bool AdvancedOptions,
	string AgeEligibility,
	string MembershipEligibility,
	object GenderEligibility,
	bool TableAllocation,
	bool ShowReserves,
	bool RequirePlayers,
	object RegistrationCompetitionType,
	object RegistrationId,
	bool RankingSubmission,
	bool FastFormat,
	bool DoublesPOTM,
	bool ExportNationalRankings,
	string NationalRankingCategory,
	bool TeamHandicap,
	int SubmissionType,
	int ApprovalType,
	bool RankingEnabled,
	bool GlobalMembers,
	bool RestrictByClub,
	string RestrictByMembership
);

internal sealed record DivisionConfig(
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
	DivisionCompetition Competition,
	DivisionBooking Booking
);

internal sealed record DivisionMatchCard(
	int CompetitionId,
	object Approval,
	object Layout,
	object PlayerLabels,
	int PrintoutLayout
);

internal sealed record DivisionOrganisation(
	int Id,
	string Name,
	string Logo,
	int Import,
	string Logins,
	IReadOnlyList<int> LoginModes,
	bool RegistrationEnabled,
	bool ReportingEnabled,
	bool CompeteMembershipRequired,
	string Website,
	string News,
	string Privacy,
	string Terms,
	string Twitter,
	string Facebook,
	string Instagram
);

internal sealed record DivisionReserves(
	int CompetitionId,
	int Type,
	int MaxStepUp,
	object MaxTeamStepUp,
	int StepUpType,
	bool TransferPlayer,
	bool MultipleTeams,
	bool OverrideEnabled
);

internal sealed record DivisionTable(
	int CompetitionId,
	bool WinLossDraw,
	bool SetsForAgainst,
	bool GamesForAgainst,
	bool PointsForAgainst,
	bool PointsAdjustments,
	bool SetsRatio,
	bool GamesRatio,
	bool PointsRatio,
	int OrderType,
	bool SetAlternative
);

internal sealed record DivisionTenant(
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
	DivisionConfig Config,
	int OrganisationId,
	DivisionOrganisation Organisation,
	bool IsTesting,
	bool CheckExpiry,
	int Type,
	object Deleted
);

