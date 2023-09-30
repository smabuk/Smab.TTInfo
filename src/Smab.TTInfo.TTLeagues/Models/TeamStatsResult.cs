namespace Smab.TTInfo.TTLeagues.Models;

internal sealed record TeamStatsResult(
	DateTimeOffset Date,
	object Games,
	int Type,
	KeyValue Team,
	int For,
	int Against,
	KeyValue Versus,
	bool Home,
	string Potm,
	int MatchId,
	int DivisionId
);
