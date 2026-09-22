namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents a player in a match, including their name, identifier, sets won, and Player of the Match status.
/// </summary>
/// <param name="Name">The name of the player.</param>
/// <param name="Id">The unique identifier for the player.</param>
/// <param name="SetsWon">The number of sets won by the player during the match.</param>
/// <param name="PoM">A value indicating whether the player was awarded Player of the Match. <see langword="true"/> if the player was the
/// Player of the Match; otherwise, <see langword="false"/>.</param>
public record MatchPlayer(string Name, int Id, double SetsWon, bool PoM, bool IsSubstitute = false);

public static class MatchPlayerExtensions
{
	extension(MatchPlayer player)
	{
		/// <summary>
		/// Gets the display name of the player, appending "(sub)" if the player is a substitute.
		/// </summary>
		/// <param name="player">The <see cref="MatchPlayer"/> instance.</param>
		/// <returns>The display name of the player.</returns>
		public string DisplayName => player.IsSubstitute ? $"{player.Name} (sub)" : player.Name;
	}
}
