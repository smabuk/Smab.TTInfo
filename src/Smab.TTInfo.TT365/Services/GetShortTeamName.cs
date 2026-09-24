namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
	/// <summary>
	/// Returns a shortened version of a team name for display purposes.
	/// </summary>
	/// <param name="teamName">The name of the team to shorten.</param>
	/// <returns>A shortened version of the team name.</returns>
	public static string GetShortNameTeamName(string teamName)
		=> teamName
			.Replace("%20", " ")
			.Replace("_", " ")
			.Replace("our lady of peace", "OLOP", StringComparison.OrdinalIgnoreCase)
			.Replace("sonning common and peppard", "SC&P", StringComparison.OrdinalIgnoreCase)
			.Replace("tilehurst methodists", "TH Methodists", StringComparison.OrdinalIgnoreCase)
			.Replace("wokingham methodist", "W Methodist", StringComparison.OrdinalIgnoreCase)
		;
}
