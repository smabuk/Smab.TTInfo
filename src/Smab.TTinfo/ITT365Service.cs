namespace Smab.TTInfo;

public interface ITT365Service
{
	string League { get; set; }
	string Season { get; set; }

	Task<FixturesView?> GetFixturesByTeamName(string TeamName = "");
	Task<FixturesView?> GetFixturesByTeamName(string TeamName = "", string? League = null, string? Season = null);
	Task<Team?> GetTeamStats(string TeamName);

}
