namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents the statistical results of a team's performance in a specific match.
/// </summary>
/// <remarks>This record encapsulates various details about a team's match, including the date,  scores, opponent
/// information, and other metadata such as the player of the match.</remarks>
/// <param name="Date"></param>
/// <param name="Games"></param>
/// <param name="Type"></param>
/// <param name="Team"></param>
/// <param name="For"></param>
/// <param name="Against"></param>
/// <param name="Versus"></param>
/// <param name="Home"></param>
/// <param name="Potm"></param>
/// <param name="MatchId"></param>
/// <param name="DivisionId"></param>
public sealed record TeamStatsResult(
	DateTimeOffset Date,
	object   Games,
	int      Type,
	IntKeyValue Team,
	int      For,
	int      Against,
	IntKeyValue Versus,
	bool     Home,
	string   Potm,
	int      MatchId,
	int      DivisionId
);
