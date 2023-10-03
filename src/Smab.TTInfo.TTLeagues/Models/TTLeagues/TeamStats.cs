namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record TeamStats(
	int?     Id,
	string   Name,
	string   ShortName,
	IntKeyValue Competition,
	IntKeyValue Division,
	IntKeyValue Club,
	IReadOnlyList<TeamStatsResult> Results,
	IReadOnlyList<TeamStatsPlayer> Players,
	object   Next,
	object   Last
 );
