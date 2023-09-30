namespace Smab.TTInfo.TTLeagues.Models;

internal sealed class Match
{
	public int Id { get; set; }
	public required MatchTeamInfo Home { get; set; }
	public required MatchTeamInfo Away { get; set; }
	public DateTimeOffset? Date { get; set; }
	public DateTimeOffset? Time { get; set; }
	public int? Week { get; set; }
	public required string Name { get; set; }
	public required string Venue { get; set; }
	public int? GroupingId { get; set; }
	public int CompetitionId { get; set; }
	public int DivisionId { get; set; }
	public long? PreviousLinkId { get; set; }
	public DateTimeOffset? Submitted { get; set; }
	public DateTimeOffset? Approved { get; set; }
	public DateTimeOffset? Rejected { get; set; }
	public DateTimeOffset? Overridden { get; set; }
	public Person? SubmittedBy { get; set; }
	public Person? ApprovedBy { get; set; }
	public Person? RejectedBy { get; set; }
	public Person? OverriddenBy { get; set; }
	public int? VenueId { get; set; }
	public DateTimeOffset? Forfeit { get; set; }
	public required string ForfeitReason { get; set; }
	public int? ForfeitId { get; set; }
	public required string AbandonedReason { get; set; }
	public DateTimeOffset? Abandoned { get; set; }
	public int LeagueId { get; set; }
	public int? ClubId { get; set; }
	public int? CountyId { get; set; }
	public required string Competition { get; set; }
	public DateTimeOffset Updated { get; set; }
	public bool Manual { get; set; }
	public DateTimeOffset Published { get; set; }
	public DateTimeOffset? Archived { get; set; }
	public Potm? Potm { get; set; }
	public int? PotmType { get; set; }
	public int Entry { get; set; }
	public bool HasReport { get; set; }
	public bool HasComments { get; set; }
	public int ResultDisplay { get; set; }
	public bool PlayAll { get; set; }
	public bool HasResults { get; set; }
	public int ScoreUp { get; set; }
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
