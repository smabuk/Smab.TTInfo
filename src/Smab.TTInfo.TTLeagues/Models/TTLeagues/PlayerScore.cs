namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a player's score in a game, including their user ID, name, and score.
/// </summary>
/// <remarks>This record is immutable and is intended to encapsulate a player's score details. The <see
/// cref="Score"/> property is nullable, allowing for scenarios where a score may not be assigned.</remarks>
/// <param name="UserId"></param>
/// <param name="Name"></param>
/// <param name="Score"></param>
public sealed record PlayerScore(
	string UserId,
	string Name,
	int Score
);
