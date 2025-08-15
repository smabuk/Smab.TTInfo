namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a team in TTLeagues, including club, members, and competition details.
/// </summary>
/// <param name="Id">The unique identifier for the team.</param>
/// <param name="CompetitionId">The competition identifier.</param>
/// <param name="Competition">The competition name.</param>
/// <param name="Name">The team name.</param>
/// <param name="Club">The club associated with the team.</param>
/// <param name="ClubId">The club identifier.</param>
/// <param name="Captain">The team captain.</param>
/// <param name="ShortName">The short name of the team.</param>
/// <param name="Description">The team description.</param>
/// <param name="CaptainId">The captain's identifier.</param>
/// <param name="Night">The night the team plays.</param>
/// <param name="Time">The time the team plays.</param>
/// <param name="VenueId">The venue identifier.</param>
/// <param name="Play">The play identifier.</param>
/// <param name="Venue">The venue details.</param>
/// <param name="BadgeId">The badge identifier.</param>
/// <param name="Abbreviation">The team abbreviation.</param>
/// <param name="Ordering">The ordering value.</param>
/// <param name="ReservesOverride">Indicates if reserves are overridden.</param>
/// <param name="Locked">Indicates if the team is locked.</param>
/// <param name="Deleted">Indicates if the team is deleted.</param>
/// <param name="DisplayName">The display name of the team.</param>
/// <param name="RetentionId">The retention identifier.</param>
/// <param name="Pool">Indicates if the team is a pool team.</param>
/// <param name="Members">The list of team members.</param>
public record Team(
	int?    Id,
	int?    CompetitionId,
	string  Competition,
	string  Name,
	Club?   Club,
	int?    ClubId,
	Person? Captain,
	string  ShortName,
	string  Description,
	string  CaptainId,
	int?    Night,
	DateTimeOffset? Time,
	int?    VenueId,
	int?    Play,
	Venue?  Venue,
	int?    BadgeId,
	string  Abbreviation,
	int?    Ordering,
	bool?   ReservesOverride,
	bool?   Locked,
	bool?   Deleted,
	string  DisplayName,
	int?    RetentionId,
	bool?   Pool,
	ImmutableList<Person>? Members
);
