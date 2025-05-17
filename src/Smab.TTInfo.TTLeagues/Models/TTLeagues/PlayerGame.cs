namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents the score of a player in a game, including points scored for and against.
/// </summary>
/// <remarks>This record is immutable and provides a concise way to store and display a player's game score. The
/// <see cref="ToString"/> method formats the score as "For-Against".</remarks>
/// <param name="For"></param>
/// <param name="Against"></param>
internal sealed record PlayerGame(
	int? For,
	int? Against
)
{
	/// <summary>
	/// Returns a string representation of the object in the format "For-Against".
	/// </summary>
	/// <returns>A string that represents the object, formatted as "For-Against".</returns>
	public override string ToString() => $"{For}-{Against}";
};
