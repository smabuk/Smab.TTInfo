namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents the result of a player's performance in a specific match or competition.
/// </summary>
/// <remarks>This record encapsulates detailed information about a player's performance, including their team,
/// opponent, game results, and various identifiers related to the match or competition. It is designed to provide a
/// comprehensive view of the player's results in a structured format.</remarks>
/// <param name="Team"></param>
/// <param name="Results"></param>
/// <param name="Opponent"></param>
/// <param name="Date"></param>
/// <param name="Games"></param>
/// <param name="Type"></param>
/// <param name="SetId"></param>
/// <param name="For"></param>
/// <param name="Against"></param>
/// <param name="Versus"></param>
/// <param name="MatchId"></param>
/// <param name="DivisionId"></param>
/// <param name="CompetitionId"></param>
/// <param name="Score"></param>
/// <param name="Rank"></param>
internal sealed record PlayerResult(
	IntKeyValue Team,
	ImmutableList<PlayerResult> Results,
	StringKeyValue Opponent,
	DateTimeOffset Date,
	ImmutableList<PlayerGame> Games,
	int         Type,
	int         SetId,
	int         For,
	int         Against,
	IntKeyValue Versus,
	int         MatchId,
	int         DivisionId,
	int         CompetitionId,
	int?        Score,
	int?        Rank
);
