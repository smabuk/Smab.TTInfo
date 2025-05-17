namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a player's national ranking in TTLeagues.
/// </summary>
/// <param name="Position">The ranking position.</param>
/// <param name="Score">The ranking score.</param>
/// <param name="Category">The ranking category.</param>
internal sealed record PlayerNationalRanking(
	int?   Position,
	int?   Score,
	string Category
);
