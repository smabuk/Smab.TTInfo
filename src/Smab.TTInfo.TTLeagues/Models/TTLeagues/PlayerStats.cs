namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record PlayerStats(
	string Id,
	IntKeyValue Competition,
	IntKeyValue Division,
	IntKeyValue Team,
	IntKeyValue Club,
	IReadOnlyList<PlayerResult> Results,
	PlayerAverage  Average,
	string   Name,
	PlayerLocalRanking    LocalRanking,
	PlayerNationalRanking NationalRanking,
	IReadOnlyList<PlayerForm> Form,
	IReadOnlyList<object> Rankings
);
