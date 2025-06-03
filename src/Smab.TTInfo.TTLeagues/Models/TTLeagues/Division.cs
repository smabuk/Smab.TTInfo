namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a division within a competition, including its metadata, configuration, and associated details.
/// </summary>
/// <remarks>This record encapsulates information about a division, such as its name, description, competition
/// association, ordering, status, and various configuration options. It also includes metadata like the last updated
/// timestamp and entrant-related details. The division is part of a larger competition structure and may include
/// additional settings for fixtures, rankings, and formats.</remarks>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="Description"></param>
/// <param name="CompetitionId"></param>
/// <param name="Competition"></param>
/// <param name="UserId"></param>
/// <param name="Ordering"></param>
/// <param name="MiniOrdering"></param>
/// <param name="Status"></param>
/// <param name="Details"></param>
/// <param name="Updated"></param>
/// <param name="StartDate"></param>
/// <param name="Reserves"></param>
/// <param name="GamesPerSet"></param>
/// <param name="GameFormat"></param>
/// <param name="FormatId"></param>
/// <param name="Format"></param>
/// <param name="PointId"></param>
/// <param name="Point"></param>
/// <param name="FixtureFormat"></param>
/// <param name="PlayEachOther"></param>
/// <param name="Deleted"></param>
/// <param name="BaseRank"></param>
/// <param name="PlayAll"></param>
/// <param name="SplitRounds"></param>
/// <param name="MiniStartingRound"></param>
/// <param name="MiniDivision"></param>
/// <param name="PreviousLinkId"></param>
/// <param name="PreviousLink"></param>
/// <param name="HalfGeneration"></param>
/// <param name="TableNos"></param>
/// <param name="TableAllocation"></param>
/// <param name="HasEntrants"></param>
/// <param name="EntrantCount"></param>
/// <param name="Days"></param>
internal sealed record Division(
	int     Id,
	string  Name,
	string  Description,
	int     CompetitionId,
	DivisionCompetition Competition,
	string  UserId,
	int     Ordering,
	object  MiniOrdering,
	int     Status,
	string  Details,
	DateTimeOffset Updated,
	object  StartDate,
	object  Reserves,
	object  GamesPerSet,
	object  GameFormat,
	object  FormatId,
	object  Format,
	object  PointId,
	object  Point,
	object  FixtureFormat,
	int?    PlayEachOther,
	object  Deleted,
	object  BaseRank,
	object  PlayAll,
	object  SplitRounds,
	object  MiniStartingRound,
	bool    MiniDivision,
	object  PreviousLinkId,
	object  PreviousLink,
	object  HalfGeneration,
	string  TableNos,
	bool?   TableAllocation,
	bool    HasEntrants,
	int     EntrantCount,
	ImmutableList<object> Days
);
