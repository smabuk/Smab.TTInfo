namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record TeamStats(
	int?     Id,
	string   Name,
	string   ShortName,
	KeyValue Competition,
	KeyValue Division,
	KeyValue Club,
	IReadOnlyList<TeamStatsResult> Results,
	IReadOnlyList<TeamStatsPlayer> Players,
	object   Next,
	object   Last
 );
