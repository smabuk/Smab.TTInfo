namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a match with detailed information about the teams, schedule, venue, competition, and related metadata.
/// </summary>
/// <remarks>This record encapsulates all relevant data for a match, including team details, scheduling
/// information, competition and league identifiers, and various statuses such as submission, approval, and forfeit
/// details. The <see cref="ActualDateTimeUtc"/> property combines the <see cref="Date"/> and <see cref="Time"/> properties
/// to provide the full date and time of the match.</remarks>
/// <param name="Id"></param>
/// <param name="Home"></param>
/// <param name="Away"></param>
/// <param name="Date"></param>
/// <param name="Time"></param>
/// <param name="Week"></param>
/// <param name="Name"></param>
/// <param name="Venue"></param>
/// <param name="GroupingId"></param>
/// <param name="CompetitionId"></param>
/// <param name="DivisionId"></param>
/// <param name="PreviousLinkId"></param>
/// <param name="Submitted"></param>
/// <param name="Approved"></param>
/// <param name="Rejected"></param>
/// <param name="Overridden"></param>
/// <param name="SubmittedBy"></param>
/// <param name="ApprovedBy"></param>
/// <param name="RejectedBy"></param>
/// <param name="OverriddenBy"></param>
/// <param name="VenueId"></param>
/// <param name="Forfeit"></param>
/// <param name="ForfeitReason"></param>
/// <param name="ForfeitId"></param>
/// <param name="AbandonedReason"></param>
/// <param name="Abandoned"></param>
/// <param name="LeagueId"></param>
/// <param name="ClubId"></param>
/// <param name="CountyId"></param>
/// <param name="Competition"></param>
/// <param name="Updated"></param>
/// <param name="Manual"></param>
/// <param name="Published"></param>
/// <param name="Archived"></param>
/// <param name="Potm"></param>
/// <param name="PotmType"></param>
/// <param name="Entry"></param>
/// <param name="HasReport"></param>
/// <param name="HasComments"></param>
/// <param name="ResultDisplay"></param>
/// <param name="PlayAll"></param>
/// <param name="HasResults"></param>
/// <param name="ScoreUp"></param>
/// <param name="Round"></param>
/// <param name="TableNo"></param>
/// <param name="DayId"></param>
/// <param name="HomeScores"></param>
/// <param name="AwayScores"></param>
/// <param name="HomeName"></param>
/// <param name="AwayName"></param>
/// <param name="Number"></param>
/// <param name="Stage"></param>
/// <param name="RoundModel"></param>
/// <param name="HomeHandicap"></param>
/// <param name="AwayHandicap"></param>
/// <param name="Bye"></param>
public sealed record Match(
	int Id,
	MatchTeamInfo Home,
	MatchTeamInfo Away,
	DateTime? Date,
	DateTime? Time,
	int? Week,
	string Name,
	string Venue,
	int? GroupingId,
	int CompetitionId,
	int DivisionId,
	long? PreviousLinkId,
	DateTime? Submitted,
	DateTime? Approved,
	DateTime? Rejected,
	DateTime? Overridden,
	Person? SubmittedBy,
	Person? ApprovedBy,
	Person? RejectedBy,
	Person? OverriddenBy,
	int? VenueId,
	DateTime? Forfeit,
	string ForfeitReason,
	int? ForfeitId,
	string AbandonedReason,
	DateTime? Abandoned,
	int LeagueId,
	int? ClubId,
	int? CountyId,
	string Competition,
	DateTime Updated,
	bool Manual,
	DateTime? Published,
	DateTime? Archived,
	Person? Potm,
	int? PotmType,
	int Entry,
	bool HasReport,
	bool HasComments,
	int ResultDisplay,
	bool PlayAll,
	bool HasResults,
	int ScoreUp,
	object? Round,
	object? TableNo,
	object? DayId,
	ImmutableList<PlayerScore> HomeScores,
	ImmutableList<PlayerScore> AwayScores,
	string HomeName,
	string AwayName,
	object? Number,
	object? Stage,
	object? RoundModel,
	object? HomeHandicap,
	object? AwayHandicap,
	object? Bye
)
{
	/// <summary>
	/// Gets the combined date and time value based on the <see cref="Date"/> and <see cref="Time"/> properties.
	/// </summary>
	public DateTime? ActualDateTimeUtc
		=> Date is null || Time is null
			? null
			: new DateTime(
					Date!.Value.Year,
					Date.Value.Month,
					Date.Value.Day,
					Time!.Value.Hour,
					Time.Value.Minute,
					0,
					DateTimeKind.Utc
				);
};
