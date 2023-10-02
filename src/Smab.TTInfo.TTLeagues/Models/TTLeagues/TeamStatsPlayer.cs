namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record TeamStatsPlayer(
	string Id,
	IReadOnlyList<TeamStatsMatch> Matches,
	double Average,
	string Name,
	int    Played,
	int    Won,
	int    Potm
);
