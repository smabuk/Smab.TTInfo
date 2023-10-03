namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record PlayerResult(
	IntKeyValue Team,
	IReadOnlyList<PlayerResult> Results,
	StringKeyValue Opponent,
	DateTimeOffset Date,
	IReadOnlyList<PlayerGame> Games,
	int      Type,
	int      SetId,
	int      For,
	int      Against,
	IntKeyValue Versus,
	int      MatchId,
	int      DivisionId,
	int      CompetitionId,
	int?     Score,
	int?     Rank
);
