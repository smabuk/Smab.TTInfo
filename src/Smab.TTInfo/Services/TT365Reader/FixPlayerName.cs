namespace Smab.TTInfo;

public partial class TT365Reader
{
	public static string FixPlayerName(string PlayerName)
	{
		string playerName = PlayerName;

		playerName = playerName.Replace("&#39;", "'");
		playerName = playerName.Replace("Osullivan", "O'Sullivan");
		playerName = playerName.Replace("Ohalloran", "O'Halloran");

		return playerName;
	}
}

