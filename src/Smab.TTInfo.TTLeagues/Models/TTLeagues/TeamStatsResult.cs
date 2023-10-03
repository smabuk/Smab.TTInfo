namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record TeamStatsResult(
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
