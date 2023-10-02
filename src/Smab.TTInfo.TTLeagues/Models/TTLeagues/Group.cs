namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record Group
(
	List<Match> Matches,
	int         Type,
	DateTimeOffset? Date,
	int         Range,
	int?        Week,
	string      Title
);
