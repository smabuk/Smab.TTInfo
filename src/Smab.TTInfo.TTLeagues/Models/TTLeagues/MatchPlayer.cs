namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a player in a match, including details about their identity, status, and match-specific attributes.
/// </summary>
/// <remarks>This record encapsulates information about a match participant, such as their user ID, name, team
/// affiliation,  and various match-related properties like forfeits, ordering, and scratch details.  Nullable
/// properties indicate optional data that may not always be available.</remarks>
/// <param name="EntrantId"></param>
/// <param name="UserId"></param>
/// <param name="Ordering"></param>
/// <param name="Name"></param>
/// <param name="PrintoutName"></param>
/// <param name="Id"></param>
/// <param name="Fixed"></param>
/// <param name="PlayerId"></param>
/// <param name="Forfeit"></param>
/// <param name="ForfeitReason"></param>
/// <param name="Type"></param>
/// <param name="Scratch"></param>
/// <param name="ScratchType"></param>
/// <param name="ScratchReason"></param>
/// <param name="TeamId"></param>
/// <param name="MembershipNo"></param>
/// <param name="DoublesPairOrdering"></param>
internal sealed record MatchPlayer(
	int?    EntrantId,
	string  UserId,
	int?    Ordering,
	string  Name,
	string  PrintoutName,
	int?    Id,
	bool?   Fixed,
	int?    PlayerId,
	object  Forfeit,
	string  ForfeitReason,
	int?    Type,
	object  Scratch,
	object  ScratchType,
	string  ScratchReason,
	int?    TeamId,
	object  MembershipNo,
	int?    DoublesPairOrdering
);
