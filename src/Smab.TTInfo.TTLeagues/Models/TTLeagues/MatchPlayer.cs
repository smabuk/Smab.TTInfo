namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

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
