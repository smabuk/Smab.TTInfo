namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a ranking entry with details about a user's position, rank, and changes over time.
/// </summary>
/// <remarks>This record is immutable and provides a snapshot of a user's ranking information,  including their
/// position, rank, and any changes relative to a starting point.</remarks>
/// <param name="Id"></param>
/// <param name="UserId"></param>
/// <param name="Position"></param>
/// <param name="Name"></param>
/// <param name="Rank"></param>
/// <param name="Start"></param>
/// <param name="Change"></param>
internal sealed record Ranking(
	 int     Id,
	 string  UserId,
	 int     Position,
	 string  Name,
	 int     Rank,
	 int     Start,
	 int     Change
);
