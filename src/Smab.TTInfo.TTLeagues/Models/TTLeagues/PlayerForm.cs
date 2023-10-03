namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record PlayerForm(
	int?     MatchId,
	IntKeyValue Versus,
	int?     Won,
	int?     Played,
	int?     Form,
	DateTimeOffset? Date
);
