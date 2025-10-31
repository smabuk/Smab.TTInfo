namespace Smab.TTInfo.TTLeagues.Pages;
public partial class TeamSummary
{
	[EditorRequired]
	[Parameter]
	public string TeamName { get; set; } = "";

	[EditorRequired]
	[Parameter]
	public string TTInfoId { get; set; } = "";

	[Parameter]
	public int TeamId { get; set; }

	public bool internalLinks = true;

	private record FixtureResult(int Id, string Result, string FullScore);

	private League? league;
	private Team? team;
	private TeamStats? teamStats;
	private List<TeamMember> teamMembers = [];
	private List<Match> fixtures = [];
	private Dictionary<string, Ranking> rankings = [];
	private readonly Dictionary<int, MatchCard> matchCards = [];
	private Dictionary<int, FixtureResult> fixtureResults = [];
	private readonly Dictionary<int, List<TeamStatsPlayer>> teamPlayersList = [];

	protected override async Task OnParametersSetAsync()
	{
		TeamName = HttpUtility.HtmlDecode(TeamName);

		fixtures = [];
		fixtureResults = [];
		StateHasChanged();
		league = await _ttleagues.GetLeague(TTInfoId);
		int competitionId = league?.CurrentCompetitions.First().Id ?? int.MinValue;
		if (TeamId <= 0) {
			TeamId = await _ttleagues.GetId(TeamName, LookupType.Team, TTInfoId, competitionId) ?? int.MinValue;
		}

		team = await _ttleagues.GetTeam(TeamId, TTInfoId);
		if (team is null) {
			return;
		}

		competitionId = team.CompetitionId ?? int.MinValue;
		teamStats = await _ttleagues.GetTeamStats(TeamId, TTInfoId, competitionId);
		teamMembers = await _ttleagues.GetTeamMembers(TeamId, TTInfoId);
		rankings = (await _ttleagues.GetRankings(TTInfoId)).ToDictionary(r => r.UserId);

		// Fixtures? allFixtures = await _ttleagues.GetAllFixturesWithMatchResults(TTInfoId, competitionId);
		Fixtures? allFixtures = await _ttleagues.GetAllFixtures(TTInfoId, competitionId);

		fixtures = allFixtures?.Matches
			.Where(f => string.Equals(f.Home.Name, TeamName, StringComparison.CurrentCultureIgnoreCase) || string.Equals(f.Away.Name, TeamName, StringComparison.CurrentCultureIgnoreCase))
			.Where(f => !(string.Equals(f.Home.Name, "Free", StringComparison.CurrentCultureIgnoreCase) || string.Equals(f.Away.Name, "Free", StringComparison.CurrentCultureIgnoreCase)))
			.OrderBy(m => m.ActualDateTime)
			.ToList() ?? [];

		foreach (Match match in fixtures) {
			if (match.HasResults) {
				MatchCard? matchCard = await _ttleagues.GetMatch(match.Id, TTInfoId);
				if (matchCard is null) {
					break;
				}

				matchCards.Add(matchCard.Id, matchCard);
				string result;
				string fullScore;
				if (matchCard.Match.Home.Name.Equals(TeamName, StringComparison.CurrentCultureIgnoreCase)) {
					result = CalculateMatchResult(matchCard.Match.Home.Score ?? 0, matchCard.Match.Away.Score ?? 0);
					fullScore = $"{matchCard.Match.Home.Score} - {matchCard.Match.Away.Score}";
				} else {
					result = CalculateMatchResult(match.Away.Score ?? 0, match.Home.Score ?? 0);
					fullScore = $"{matchCard.Match.Away.Score} - {matchCard.Match.Home.Score}";
				}

				_ = fixtureResults.TryAdd(matchCard.Id, new(matchCard.Id, result, fullScore));
			}
		}

		StateHasChanged();

		List<int> teamList = [.. fixtures
			.Select(t => t.Home.TeamId ?? 0)
			.Union(fixtures.Select(t => t.Away.TeamId ?? 0))
			.Where(id => id != TeamId)
			.Distinct()];
		List<Task<IEnumerable<TeamStatsPlayer>>> tasks = [];
		foreach (int id in teamList) {
			tasks.Add(GetTeamPlayers(id, competitionId));
		}

		await foreach (Task task in Task.WhenEach(tasks)) {
			StateHasChanged();
		}

	}

	internal async Task<IEnumerable<TeamStatsPlayer>> GetTeamPlayers(int teamId, int competitionId)
	{
		List<TeamStatsPlayer>? players = [];
		if (teamPlayersList.TryGetValue(teamId, out players)) {
			return players;
		} else {
			players = (await _ttleagues.GetTeamStats(teamId, TTInfoId, competitionId))?.Players.ToList();
			if (players is null) {
				return [];
			}

			List<TeamMember> members = await _ttleagues.GetTeamMembers(teamId, TTInfoId);

			List<TeamStatsPlayer> playersWithoutResults = [.. members.ExceptBy(players.Select(p => p.Id), m => m.MemberId)
				.Select(m => new TeamStatsPlayer(
					m.MemberId,
					[],
					0.0,
					m.Name,
					0,
					0,
					0
				))];

			players = [.. players.Union(playersWithoutResults)];

			_ = teamPlayersList.TryAdd(teamId, players);
			return players;
		}
	}

	static string CalculateForm(IEnumerable<TeamStatsMatch> matches)
		=> String.Join(",", matches.Take(6).Select(m => $"{m.Form}"));

	static string? CalculateDoublesWinners(MatchCard matchCard)
	{
		int HomeTotal = matchCard.Results.Home.Select(m => m.Score).Sum();
		int AwayTotal = matchCard.Results.Away.Select(m => m.Score).Sum();

		if (matchCard.Match.Home.Score - HomeTotal >= 1 && matchCard.Match.Away.Score == AwayTotal) {
			return $"Doubles: {matchCard.Match.Home.Name}";
		}

		if (matchCard.Match.Away.Score - AwayTotal >= 1 && matchCard.Match.Home.Score == HomeTotal) {
			return $"Doubles: {matchCard.Match.Away.Name}";
		}

		return null;
	}

	string CalculateMatchHref(Match match)
	{
		return internalLinks
			? $"TTLeagues/Match/{TTInfoId}/{match.Id}"
			: $"https://{TTInfoId}.ttleagues.com/league/{match.CompetitionId}/division/{match.DivisionId}/match/{match.Id}";
	}


	string CalculateResultHref(TeamStats teamStats, TeamStatsResult result)
	{
		return internalLinks
			? $"TTLeagues/Match/{TTInfoId}/{result.MatchId}"
			: $"https://{TTInfoId}.ttleagues.com/league/{teamStats.Competition.Key}/division/{result.DivisionId}/match/{result.MatchId}";
	}

	static string CalculateMatchResult(int scoreFor, int scoreAgainst)
	{
		return (scoreFor - scoreAgainst) switch
		{
			> 0 => "win",
			< 0 => "loss",
			0 => "draw",
		};
	}

	bool MatchHasDefaultStartTime(Match match) => match.Time?.TimeOfDay == TTLeaguesReader.DEFAULT_START_TIME.ToTimeSpan();
}
