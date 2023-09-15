#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.Text.Json.Serialization;

namespace Smab.TTInfo.Models.TTLeagues;

public partial class All
{
	[JsonPropertyName("id")]
	public long Id { get; set; }

	[JsonPropertyName("leagueId")]
	public long LeagueId { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("menuName")]
	public string MenuName { get; set; }

	[JsonPropertyName("startDate")]
	public object StartDate { get; set; }

	[JsonPropertyName("startTime")]
	public object StartTime { get; set; }

	[JsonPropertyName("userId")]
	public string UserId { get; set; }

	[JsonPropertyName("formatId")]
	public object FormatId { get; set; }

	[JsonPropertyName("gamesPerSet")]
	public object GamesPerSet { get; set; }

	[JsonPropertyName("description")]
	public string Description { get; set; }

	[JsonPropertyName("details")]
	public string Details { get; set; }

	[JsonPropertyName("pointId")]
	public object PointId { get; set; }

	[JsonPropertyName("minAge")]
	public object MinAge { get; set; }

	[JsonPropertyName("maxAge")]
	public object MaxAge { get; set; }

	[JsonPropertyName("organiserId")]
	public string OrganiserId { get; set; }

	[JsonPropertyName("organiser")]
	public object Organiser { get; set; }

	[JsonPropertyName("matchCard")]
	public object MatchCard { get; set; }

	[JsonPropertyName("table")]
	public object Table { get; set; }

	[JsonPropertyName("averages")]
	public object Averages { get; set; }

	[JsonPropertyName("reserves")]
	public object Reserves { get; set; }

	[JsonPropertyName("gameFormat")]
	public object GameFormat { get; set; }

	[JsonPropertyName("endDate")]
	public object EndDate { get; set; }

	[JsonPropertyName("playEachOther")]
	public long PlayEachOther { get; set; }

	[JsonPropertyName("playAll")]
	public bool PlayAll { get; set; }

	[JsonPropertyName("ready")]
	public object Ready { get; set; }

	[JsonPropertyName("deleted")]
	public object Deleted { get; set; }

	[JsonPropertyName("published")]
	public object Published { get; set; }

	[JsonPropertyName("archived")]
	public object Archived { get; set; }

	[JsonPropertyName("hidden")]
	public object Hidden { get; set; }

	[JsonPropertyName("fixtureFormat")]
	public long FixtureFormat { get; set; }

	[JsonPropertyName("resultDisplay")]
	public long ResultDisplay { get; set; }

	[JsonPropertyName("league")]
	public string League { get; set; }

	[JsonPropertyName("night")]
	public object Night { get; set; }

	[JsonPropertyName("manualScoring")]
	public bool ManualScoring { get; set; }

	[JsonPropertyName("ranking")]
	public bool Ranking { get; set; }

	[JsonPropertyName("tableCount")]
	public object TableCount { get; set; }

	[JsonPropertyName("days")]
	public object Days { get; set; }

	[JsonPropertyName("submissionPeriods")]
	public object SubmissionPeriods { get; set; }

	[JsonPropertyName("splitRounds")]
	public bool SplitRounds { get; set; }

	[JsonPropertyName("captainType")]
	public long CaptainType { get; set; }

	[JsonPropertyName("playerType")]
	public long PlayerType { get; set; }

	[JsonPropertyName("groupId")]
	public object GroupId { get; set; }

	[JsonPropertyName("group")]
	public object Group { get; set; }

	[JsonPropertyName("groupOrdering")]
	public long GroupOrdering { get; set; }

	[JsonPropertyName("halfGeneration")]
	public bool HalfGeneration { get; set; }

	[JsonPropertyName("advancedOptions")]
	public bool AdvancedOptions { get; set; }

	[JsonPropertyName("tableAllocation")]
	public bool TableAllocation { get; set; }

	[JsonPropertyName("showReserves")]
	public bool ShowReserves { get; set; }

	[JsonPropertyName("requirePlayers")]
	public bool RequirePlayers { get; set; }

	[JsonPropertyName("rankOrder")]
	public bool RankOrder { get; set; }

	[JsonPropertyName("ageEligibility")]
	public object AgeEligibility { get; set; }

	[JsonPropertyName("membershipEligibility")]
	public object MembershipEligibility { get; set; }

	[JsonPropertyName("genderEligibility")]
	public object GenderEligibility { get; set; }

	[JsonPropertyName("registrationCompetitionType")]
	public object RegistrationCompetitionType { get; set; }

	[JsonPropertyName("registrationId")]
	public object RegistrationId { get; set; }

	[JsonPropertyName("rankingSubmission")]
	public bool RankingSubmission { get; set; }

	[JsonPropertyName("doublesPOTM")]
	public bool DoublesPotm { get; set; }

	[JsonPropertyName("fastFormat")]
	public bool FastFormat { get; set; }

	[JsonPropertyName("exportNationalRankings")]
	public bool ExportNationalRankings { get; set; }

	[JsonPropertyName("nationalRankingCategory")]
	public string NationalRankingCategory { get; set; }

	[JsonPropertyName("printoutFormat")]
	public long PrintoutFormat { get; set; }

	[JsonPropertyName("teamHandicap")]
	public bool TeamHandicap { get; set; }
}
