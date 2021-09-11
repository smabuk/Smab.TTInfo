namespace Smab.TTInfo;

public class TT365ReaderHelper {
	private const string CurrentSeason = "Senior_2021-22";

	static readonly Dictionary<string, List<string>> TeamPlayersList = new();

	public static async Task<TT365Models.FixturesView> GetFixturesList(ITT365Service _tt365, string TeamName = "") {
		_tt365.Season = CurrentSeason;

		return await _tt365.GetFixturesAdvancedView(TeamName) ?? new();
	}

	public static async Task<TT365Models.Team> GetTeam(ITT365Service _tt365, string TeamName) {
		_tt365.Season = CurrentSeason;

		TT365Models.Team team = await _tt365.GetTeamStats(TeamName) ?? new();
		List<string>? teamplayers = (from p in team.Players
						   select p.Name + " (" + p.WinPercentage + ")").ToList();
		TeamPlayersList.TryAdd(TeamName, teamplayers);

		return team;
	}

	public static async Task<List<string>> GetTeamPlayersList(ITT365Service _tt365, string TeamName) {
		if (!TeamPlayersList.ContainsKey(TeamName)) {
			_ = await GetTeam(_tt365, TeamName);
		}

		return TeamPlayersList[TeamName];
	}

}
