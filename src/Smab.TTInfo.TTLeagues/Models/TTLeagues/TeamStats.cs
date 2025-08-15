namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents the statistics and related information for a sports team, including results, players, and associated
/// metadata.
/// </summary>
/// <remarks>This record encapsulates detailed information about a team, such as its name, competition, division,
/// and club associations. It also includes collections of results and players, as well as metadata for the next and
/// last events.</remarks>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="ShortName"></param>
/// <param name="Competition"></param>
/// <param name="Division"></param>
/// <param name="Club"></param>
/// <param name="Results"></param>
/// <param name="Players"></param>
/// <param name="Next"></param>
/// <param name="Last"></param>
public sealed record TeamStats(
	int?     Id,
	string   Name,
	string   ShortName,
	IntKeyValue Competition,
	IntKeyValue Division,
	IntKeyValue Club,
	ImmutableList<TeamStatsResult> Results,
	ImmutableList<TeamStatsPlayer> Players,
	object   Next,
	object   Last
 );
