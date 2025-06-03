namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents information about a team in a match, including its members, score, and other related details.
/// </summary>
/// <remarks>This record encapsulates various properties of a team, such as its identifier, name, members, score,
/// and captain. It is designed to provide a comprehensive view of a team's state within the context of a
/// match.</remarks>
/// <param name="Id"></param>
/// <param name="UserId"></param>
/// <param name="TeamId"></param>
/// <param name="Name"></param>
/// <param name="ShortName"></param>
/// <param name="Members"></param>
/// <param name="Score"></param>
/// <param name="Reserves"></param>
/// <param name="Type"></param>
/// <param name="Points"></param>
/// <param name="Captain"></param>
/// <param name="ClubId"></param>
/// <param name="CaptainId"></param>
/// <param name="DisplayName"></param>
internal sealed record MatchTeamInfo(
	int     Id,
	string  UserId,
	int?    TeamId,
	string  Name,
	string  ShortName,
	ImmutableList<Member>? Members,
	int?    Score,
	ImmutableList<Member>? Reserves,
	int     Type,
	int?    Points,
	string  Captain,
	int?    ClubId,
	string  CaptainId,
	string  DisplayName
);
