namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a player's local ranking within a specific category.
/// </summary>
/// <remarks>This record encapsulates the player's position and score in a local ranking system, along with the
/// category to which the ranking applies. The position and score may be null if the player is not ranked or if the data
/// is unavailable.</remarks>
/// <param name="Position">The player's position in the local ranking. A lower value indicates a higher rank. This value is null if the player
/// is not ranked.</param>
/// <param name="Score">The player's score in the local ranking. This value is null if the score is unavailable or the player is not ranked.</param>
/// <param name="Category">The category or context of the ranking, such as a game mode or competition type. This value cannot be null.</param>
internal sealed record PlayerLocalRanking(
	int?   Position,
	int?   Score,
	string Category
);
