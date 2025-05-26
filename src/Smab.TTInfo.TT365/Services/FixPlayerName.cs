namespace Smab.TTInfo.TT365.Services;

/// <summary>
/// Provides utility methods for fixing player names for TT365.
/// </summary>
public sealed partial class TT365Reader
{
	/// <summary>
	/// Fixes known issues in player names (e.g., missing apostrophes).
	/// </summary>
	/// <param name="playerName">The original player name.</param>
	/// <returns>The fixed player name.</returns>
	public static string FixPlayerName(string playerName)
	{
		playerName = HttpUtility.HtmlDecode(playerName).Trim();

		playerName = playerName.Replace("OSullivan", "O'Sullivan", StringComparison.InvariantCultureIgnoreCase);
		playerName = playerName.Replace("OHalloran", "O'Halloran", StringComparison.InvariantCultureIgnoreCase);

		return playerName;
	}
}
