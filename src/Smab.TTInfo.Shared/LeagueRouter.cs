namespace Smab.TTInfo.Shared;
public class LeagueRouter
{
	// TODO: remove the hard coding for this later
	private readonly static Dictionary<string, LeagueType> _leagueMap = new(StringComparer.CurrentCultureIgnoreCase)
	{
		{ "Maidenhead"           , LeagueType.TTLeagues },
		{ "Reading"              , LeagueType.TT365 },
		{ "BracknellAndWokingham", LeagueType.TT365 },
	};

	public static LeagueType GetLeagueType(string ttinfoId)
	{
		return _leagueMap.TryGetValue(ttinfoId, out var leagueType)
			? leagueType
			: LeagueType.TTLeagues;
	}

	public enum LeagueType
	{
		TT365,
		TTLeagues
	}
}
