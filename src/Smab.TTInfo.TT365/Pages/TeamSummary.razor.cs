using Smab.TTInfo.TT365.Models.TT365;

namespace Smab.TTInfo.TT365.Pages;
public partial class TeamSummary
{
	[EditorRequired]
	[Parameter]
	public string TeamId { get; set; } = "";

	[EditorRequired]
	[Parameter]
	public string LeagueId { get; set; } = "";

	private record FixtureResult(int Id, string Result, string FullScore);

	public string TeamName { get; set; } = "";
	private Team? team;
	private List<Fixture> fixtures = [];
	private readonly Dictionary<string, List<Player>> teamPlayersList = [];
	private Dictionary<int, FixtureResult> fixtureResults = [];
	private League? league;

	protected override async Task OnParametersSetAsync()
	{
		TeamName = TeamId.Replace("_", " ");
		team = null;
		fixtures = [];
		fixtureResults = [];
		StateHasChanged();
		league = await _tt365.GetLeague((TT365LeagueId)LeagueId);
		team = await _tt365.GetTeamStats((TT365LeagueId)LeagueId, TeamName);
		if (team is not null) {

			fixtures = [.. (await _tt365.GetAllFixtures((TT365LeagueId)LeagueId, league?.GetCurrentSeasonId()) ?? []).Where(f => string.Equals(f.HomeTeam, TeamName, StringComparison.CurrentCultureIgnoreCase) || string.Equals(f.AwayTeam, TeamName, StringComparison.CurrentCultureIgnoreCase))];
			foreach (Fixture fixture in fixtures) {
				if (fixture is CompletedFixture completedFixture) {
					string result;
					string fullScore;
					if (fixture.HomeTeam.Equals(@team.Name)) {
						result = (completedFixture.ForHome > completedFixture.ForAway) ? "win" : (completedFixture.ForHome < completedFixture.ForAway) ? "loss" : "draw";
						fullScore = $"{completedFixture.ForHome} - {completedFixture.ForAway}";
					} else {
						result = (completedFixture.ForHome > completedFixture.ForAway) ? "loss" : (completedFixture.ForHome < completedFixture.ForAway) ? "win" : "draw";
						fullScore = $"{completedFixture.ForAway} - {completedFixture.ForHome}";
					}

					_ = fixtureResults.TryAdd(completedFixture.Id, new(completedFixture.Id, result, fullScore));
				}
			}

			StateHasChanged();

			List<string> teamList = [.. fixtures
				.Select(t => t.HomeTeam)
				.Union(fixtures
						.Select(t => t.HomeTeam))
						.Distinct()
						.Where(t => t != TeamId)];
			List<Task<IEnumerable<Player>>> tasks = [];
			foreach (string team in teamList) {
				tasks.Add(GetTeamPlayers(team));
			}

			await foreach (Task task in Task.WhenEach(tasks)) {
				StateHasChanged();
			}
		}
	}

	private string DisplayHomeOrAwayIfSameClub(Fixture fixture)
	{
		if (team is null) {
			return "";
		}

		if (fixture.HomeTeam[..^2] == fixture.AwayTeam[..^2]) {
			return fixture.HomeTeam.Equals(team.Name) ? "home" : "away";
		} else {
			return "";
		}
	}

	protected async Task<IEnumerable<Player>> GetTeamPlayers(string teamName)
	{
		if (teamPlayersList.TryGetValue(teamName, out List<Player>? value)) {
			return value;
		} else {
			ICollection<Player>? players = (await _tt365.GetTeamStats((TT365LeagueId)LeagueId, teamName))?.Players;
			if (players is null) {
				return [];
			}

			_ = teamPlayersList.TryAdd(teamName, [.. players]);
			return players;
		}
	}

	private static string PlayersAverages(IEnumerable<Player> players)
	{
		IEnumerable<string> playersList = players
			.OrderByDescending(p => p.WinPercentage)
			.Select(p => $"{p.Name} ({Math.Round(p.WinPercentage, 0)}%)");
		return String.Join(", ", playersList);
	}
}
