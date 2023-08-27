namespace Smab.TTInfo;

public sealed partial class TT365Reader
{
	public static string FixPlayerName(string PlayerName)
	{
		string playerName = PlayerName;

		playerName = playerName.Replace("&#39;", "'");
		playerName = playerName.Replace("OSullivan", "O'Sullivan");
		playerName = playerName.Replace("Osullivan", "O'Sullivan");
		playerName = playerName.Replace("OHalloran", "O'Halloran");
		playerName = playerName.Replace("Ohalloran", "O'Halloran");

		return playerName;
	}
}
