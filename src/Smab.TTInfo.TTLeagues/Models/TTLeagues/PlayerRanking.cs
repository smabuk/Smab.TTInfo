namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents the ranking of a player within a specific category, including their position and score.
/// </summary>
/// <remarks>This record is used to encapsulate a player's ranking details, such as their position in a
/// leaderboard, their score, and the category in which they are ranked. The <see cref="Position"/> and <see
/// cref="Score"/> properties are nullable to accommodate cases where ranking or scoring data may not be
/// available.</remarks>
/// <param name="Position">The player's position in the ranking. A lower value typically indicates a higher rank. If <c>null</c>, the player's
/// position is not available.</param>
/// <param name="Score">The player's score associated with the ranking. If <c>null</c>, the player's score is not available.</param>
/// <param name="Category">The category in which the player is ranked. This is a non-null string that identifies the context of the ranking,
/// such as "Sports", "Gaming", or "Academics".</param>
internal sealed record PlayerRanking(
	int?   Position,
	int?   Score,
	string Category
);
