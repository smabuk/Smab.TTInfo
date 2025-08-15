namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a competition with various attributes and settings, such as league association,  scheduling, format,
/// eligibility, and organizational details.
/// </summary>
/// <remarks>This record encapsulates the data and configuration for a competition, including its  identification,
/// scheduling, format, and eligibility criteria. It is designed to support  a wide range of competition types and
/// configurations, such as team or individual events,  manual or automated scoring, and advanced options like table
/// allocation and ranking submissions.</remarks>
/// <param name="Id"></param>
/// <param name="LeagueId"></param>
/// <param name="Name"></param>
/// <param name="MenuName"></param>
/// <param name="StartDate"></param>
/// <param name="StartTime"></param>
/// <param name="UserId"></param>
/// <param name="FormatId"></param>
/// <param name="GamesPerSet"></param>
/// <param name="Description"></param>
/// <param name="Details"></param>
/// <param name="PointId"></param>
/// <param name="MinAge"></param>
/// <param name="MaxAge"></param>
/// <param name="OrganiserId"></param>
/// <param name="Organiser"></param>
/// <param name="MatchCard"></param>
/// <param name="Table"></param>
/// <param name="Averages"></param>
/// <param name="Reserves"></param>
/// <param name="GameFormat"></param>
/// <param name="EndDate"></param>
/// <param name="PlayEachOther"></param>
/// <param name="PlayAll"></param>
/// <param name="Ready"></param>
/// <param name="Deleted"></param>
/// <param name="Published"></param>
/// <param name="Archived"></param>
/// <param name="Hidden"></param>
/// <param name="FixtureFormat"></param>
/// <param name="ResultDisplay"></param>
/// <param name="League"></param>
/// <param name="Night"></param>
/// <param name="ManualScoring"></param>
/// <param name="Ranking"></param>
/// <param name="TableCount"></param>
/// <param name="Days"></param>
/// <param name="SubmissionPeriods"></param>
/// <param name="SplitRounds"></param>
/// <param name="CaptainType"></param>
/// <param name="PlayerType"></param>
/// <param name="GroupId"></param>
/// <param name="Group"></param>
/// <param name="GroupOrdering"></param>
/// <param name="HalfGeneration"></param>
/// <param name="AdvancedOptions"></param>
/// <param name="TableAllocation"></param>
/// <param name="ShowReserves"></param>
/// <param name="RequirePlayers"></param>
/// <param name="RankOrder"></param>
/// <param name="AgeEligibility"></param>
/// <param name="MembershipEligibility"></param>
/// <param name="GenderEligibility"></param>
/// <param name="RegistrationCompetitionType"></param>
/// <param name="RegistrationId"></param>
/// <param name="RankingSubmission"></param>
/// <param name="DoublesPOTM"></param>
/// <param name="FastFormat"></param>
/// <param name="ExportNationalRankings"></param>
/// <param name="NationalRankingCategory"></param>
/// <param name="PrintoutFormat"></param>
/// <param name="TeamHandicap"></param>
public sealed record Competition(
	int     Id,
	int     LeagueId,
	string  Name,
	string  MenuName,
	DateTimeOffset? StartDate,
	DateTimeOffset? StartTime,
	string  UserId,
	int?    FormatId,
	int?    GamesPerSet,
	string  Description,
	string  Details,
	int?    PointId,
	int?    MinAge,
	int?    MaxAge,
	string  OrganiserId,
	Person? Organiser,
	object  MatchCard,
	object  Table,
	object  Averages,
	object  Reserves,
	int?    GameFormat,
	DateTimeOffset? EndDate,
	int     PlayEachOther,
	bool    PlayAll,
	DateTimeOffset? Ready,
	DateTimeOffset? Deleted,
	DateTimeOffset? Published,
	DateTimeOffset? Archived,
	DateTimeOffset? Hidden,
	int     FixtureFormat,
	int     ResultDisplay,
	string  League,
	object  Night,
	bool    ManualScoring,
	bool    Ranking,
	object  TableCount,
	object  Days,
	object  SubmissionPeriods,
	bool    SplitRounds,
	int     CaptainType,
	int     PlayerType,
	object  GroupId,
	object  Group,
	int?    GroupOrdering,
	bool    HalfGeneration,
	bool    AdvancedOptions,
	bool    TableAllocation,
	bool    ShowReserves,
	bool    RequirePlayers,
	bool    RankOrder,
	object  AgeEligibility,
	object  MembershipEligibility,
	object  GenderEligibility,
	object  RegistrationCompetitionType,
	object  RegistrationId,
	bool    RankingSubmission,
	bool    DoublesPOTM,
	bool    FastFormat,
	bool    ExportNationalRankings,
	string  NationalRankingCategory,
	int     PrintoutFormat,
	bool    TeamHandicap
);
