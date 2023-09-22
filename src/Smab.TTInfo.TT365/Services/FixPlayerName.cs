namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
	public static string FixPlayerName(string PlayerName)
	{
		string playerName = PlayerName;

		playerName = HttpUtility.HtmlDecode(playerName);
		playerName = playerName.Replace("OSullivan", "O'Sullivan", StringComparison.InvariantCultureIgnoreCase);
		playerName = playerName.Replace("OHalloran", "O'Halloran", StringComparison.InvariantCultureIgnoreCase);

		return playerName;
	}
}
