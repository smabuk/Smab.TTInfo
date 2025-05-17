namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents the average performance statistics of a player, including the number of games played and won.
/// </summary>
/// <remarks>This record is used to encapsulate optional performance metrics for a player.  Both properties are
/// nullable, allowing for scenarios where the data may be incomplete or unavailable.</remarks>
/// <param name="Played">The total number of games the player has participated in. Can be <see langword="null"/> if the data is unavailable.</param>
/// <param name="Won">The total number of games the player has won. Can be <see langword="null"/> if the data is unavailable.</param>
internal sealed record PlayerAverage(
	int? Played,
	int? Won
);
