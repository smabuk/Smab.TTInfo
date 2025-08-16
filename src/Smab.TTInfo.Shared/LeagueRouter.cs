namespace Smab.TTInfo.Shared;
public class LeagueRouter
{
	public static LeagueType GetLeagueType(string ttinfoId)
	{
		// TODO: remove the hard coding for this later
		return ttinfoId.ToUpperInvariant() switch
		{
			"READING"   => LeagueType.TT365,
			"BRACKNELL" => LeagueType.TT365,
			_ => LeagueType.TTLeagues
		};
	}

	public enum LeagueType
	{
		TT365,
		TTLeagues
	}
}
