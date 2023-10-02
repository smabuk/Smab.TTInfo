namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record MatchTeamInfo(
	int     Id,
	string  UserId,
	int?    TeamId,
	string  Name,
	string  ShortName,
	IReadOnlyList<Member>? Members,
	int?    Score,
	IReadOnlyList<object>? Reserves,
	int     Type,
	int?    Points,
	string  Captain,
	int?    ClubId,
	string  CaptainId,
	string  DisplayName
);
