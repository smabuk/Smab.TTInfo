namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a division competition with detailed configuration and metadata.
/// </summary>
/// <remarks>This record encapsulates all the properties and settings required to define a division competition, 
/// including its structure, rules, eligibility criteria, and associated entities such as leagues, formats,  and match
/// cards. It is designed to support advanced competition management scenarios.</remarks>
/// <param name="Id"></param>
/// <param name="OldLeagueId"></param>
/// <param name="OldLeague"></param>
/// <param name="Name"></param>
/// <param name="MenuName"></param>
/// <param name="StartDate"></param>
/// <param name="UserId"></param>
/// <param name="FormatId"></param>
/// <param name="Format"></param>
/// <param name="GamesPerSet"></param>
/// <param name="GameFormat"></param>
/// <param name="Description"></param>
/// <param name="Details"></param>
/// <param name="PointId"></param>
/// <param name="MinAge"></param>
/// <param name="MaxAge"></param>
/// <param name="MatchCard"></param>
/// <param name="Table"></param>
/// <param name="Averages"></param>
/// <param name="Reserves"></param>
/// <param name="EndDate"></param>
/// <param name="OrganiserId"></param>
/// <param name="Organiser"></param>
/// <param name="PlayAll"></param>
/// <param name="PlayEachOther"></param>
/// <param name="Deleted"></param>
/// <param name="Ready"></param>
/// <param name="Published"></param>
/// <param name="StartTime"></param>
/// <param name="Archived"></param>
/// <param name="Hidden"></param>
/// <param name="FixtureFormat"></param>
/// <param name="ResultDisplay"></param>
/// <param name="Night"></param>
/// <param name="TableCount"></param>
/// <param name="ManualScoring"></param>
/// <param name="Season"></param>
/// <param name="Ranking"></param>
/// <param name="TenantId"></param>
/// <param name="Tenant"></param>
/// <param name="GroupId"></param>
/// <param name="Group"></param>
/// <param name="GroupOrdering"></param>
/// <param name="SplitRounds"></param>
/// <param name="CaptainType"></param>
/// <param name="PlayerType"></param>
/// <param name="HalfGeneration"></param>
/// <param name="AdvancedOptions"></param>
/// <param name="AgeEligibility"></param>
/// <param name="MembershipEligibility"></param>
/// <param name="GenderEligibility"></param>
/// <param name="TableAllocation"></param>
/// <param name="ShowReserves"></param>
/// <param name="RequirePlayers"></param>
/// <param name="RegistrationCompetitionType"></param>
/// <param name="RegistrationId"></param>
/// <param name="RankingSubmission"></param>
/// <param name="FastFormat"></param>
/// <param name="DoublesPOTM"></param>
/// <param name="ExportNationalRankings"></param>
/// <param name="NationalRankingCategory"></param>
/// <param name="TeamHandicap"></param>
/// <param name="SubmissionType"></param>
/// <param name="ApprovalType"></param>
/// <param name="RankingEnabled"></param>
/// <param name="GlobalMembers"></param>
/// <param name="RestrictByClub"></param>
/// <param name="RestrictByMembership"></param>
internal sealed record DivisionCompetition(
	int     Id,
	int     OldLeagueId,
	object  OldLeague,
	string  Name,
	string  MenuName,
	DateTimeOffset StartDate,
	string  UserId,
	int     FormatId,
	object  Format,
	int     GamesPerSet,
	int     GameFormat,
	string  Description,
	string  Details,
	int     PointId,
	object  MinAge,
	object  MaxAge,
	DivisionMatchCard MatchCard,
	DivisionTable     Table,
	DivisionAverages  Averages,
	DivisionReserves  Reserves,
	object  EndDate,
	string  OrganiserId,
	object  Organiser,
	bool    PlayAll,
	int     PlayEachOther,
	object  Deleted,
	DateTimeOffset Ready,
	DateTimeOffset? Published,
	object  StartTime,
	object  Archived,
	object  Hidden,
	int     FixtureFormat,
	int     ResultDisplay,
	object  Night,
	object  TableCount,
	bool    ManualScoring,
	int     Season,
	bool    Ranking,
	int     TenantId,
	DivisionTenant Tenant,
	object  GroupId,
	object  Group,
	object  GroupOrdering,
	bool    SplitRounds,
	int     CaptainType,
	int     PlayerType,
	bool    HalfGeneration,
	bool    AdvancedOptions,
	string  AgeEligibility,
	string  MembershipEligibility,
	object  GenderEligibility,
	bool    TableAllocation,
	bool    ShowReserves,
	bool    RequirePlayers,
	object  RegistrationCompetitionType,
	object  RegistrationId,
	bool    RankingSubmission,
	bool    FastFormat,
	bool    DoublesPOTM,
	bool    ExportNationalRankings,
	string  NationalRankingCategory,
	bool    TeamHandicap,
	int     SubmissionType,
	int     ApprovalType,
	bool    RankingEnabled,
	bool    GlobalMembers,
	bool    RestrictByClub,
	string  RestrictByMembership
);

