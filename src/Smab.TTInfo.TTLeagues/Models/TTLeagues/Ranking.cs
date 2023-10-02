namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record Ranking(
	 int     Id,
	 string  UserId,
	 int     Position,
	 string  Name,
	 int     Rank,
	 int     Start,
	 int     Change
);
