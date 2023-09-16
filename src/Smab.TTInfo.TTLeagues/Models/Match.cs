namespace Smab.TTInfo.TTLeagues.Models;

public partial class Match
{
	public long Id { get; set; }
	public required MatchTeamInfo Home { get; set; }
	public required MatchTeamInfo Away { get; set; }
	public DateTime Date { get; set; }
	public DateTime? Time { get; set; }
	public long Week { get; set; }
	public required string Name { get; set; }
	public required string Venue { get; set; }
	public long? GroupingId { get; set; }
	public long CompetitionId { get; set; }
	public long DivisionId { get; set; }
	public long? PreviousLinkId { get; set; }
	public DateTime? Submitted { get; set; }
	public DateTime? Approved { get; set; }
	public DateTime? Rejected { get; set; }
	public DateTime? Overridden { get; set; }
	public ActionedBy? SubmittedBy { get; set; }
	public ActionedBy? ApprovedBy { get; set; }
	public ActionedBy? RejectedBy { get; set; }
	public ActionedBy? OverriddenBy { get; set; }
	public long? VenueId { get; set; }
	public DateTime? Forfeit { get; set; }
	public required string ForfeitReason { get; set; }
	public long? ForfeitId { get; set; }
	public required string AbandonedReason { get; set; }
	public DateTime? Abandoned { get; set; }
	public long LeagueId { get; set; }
	public long? ClubId { get; set; }
	public long? CountyId { get; set; }
	public required string Competition { get; set; }
	public DateTime Updated { get; set; }
	public bool Manual { get; set; }
	public DateTime Published { get; set; }
	public DateTime? Archived { get; set; }
	public object? Potm { get; set; }
	public long? PotmType { get; set; }
	public long Entry { get; set; }
	public bool HasReport { get; set; }
	public bool HasComments { get; set; }
	public long ResultDisplay { get; set; }
	public bool PlayAll { get; set; }
	public bool HasResults { get; set; }
	public long ScoreUp { get; set; }
	public object? Round { get; set; }
	public object? TableNo { get; set; }
	public object? DayId { get; set; }
	public required List<MatchPlayer> HomeScores { get; set; }
	public required List<MatchPlayer> AwayScores { get; set; }
	public required string HomeName { get; set; }
	public required string AwayName { get; set; }
	public object? Number { get; set; }
	public object? Stage { get; set; }
	public object? RoundModel { get; set; }
	public object? HomeHandicap { get; set; }
	public object? AwayHandicap { get; set; }
}
