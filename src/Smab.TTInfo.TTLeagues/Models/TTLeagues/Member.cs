namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record Member(
	int?    TeamId,
	string  MemberId,
	string  Name,
	int?    Ordering,
	bool?   Playing,
	int?    TotalStepUp,
	object? ImageId,
	string  Team,
	bool?   Disabled
);
