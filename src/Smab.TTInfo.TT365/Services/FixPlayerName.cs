namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
	/// <summary>
	/// Fixes known issues in player names (e.g., missing apostrophes).
	/// </summary>
	/// <param name="playerName">The original player name.</param>
	/// <returns>The fixed player name.</returns>
	public static string FixPlayerName(string playerName)
	{
		return HttpUtility.HtmlDecode(playerName)
			.Trim()
			.Replace("OSullivan", "O'Sullivan", StringComparison.OrdinalIgnoreCase)
			.Replace("OHalloran", "O'Halloran", StringComparison.OrdinalIgnoreCase)
			.Replace("Kashif Subhan", "Kash Subhan", StringComparison.OrdinalIgnoreCase);
	}
}
