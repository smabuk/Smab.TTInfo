namespace Smab.TTInfo.TT365.Pages;
public partial class RecentFixtures
{
	[Parameter]
	public int? NoOfFixtures { get; set; }

	[EditorRequired]
	[Parameter]
	public string LeagueId { get; set; } = "";

	private List<Fixture>? fixtures;
	private readonly Dictionary<int, FixtureResult> fixtureResults = [];
	private League? league;
	private TT365SeasonId seasonId;
	private TT365LeagueId leagueId;

	protected override async Task OnParametersSetAsync()
	{
		NoOfFixtures ??= 15;
		fixtures = null;
		StateHasChanged();
		league = await _tt365.GetLeague((TT365LeagueId)LeagueId);
		DateOnly today = DateOnly.FromDateTime(timeProvider.GetLocalNow().DateTime);
		seasonId = league?.CurrentSeasonId ?? default;
		leagueId = (TT365LeagueId)LeagueId;
		fixtures = [.. (await _tt365.GetAllFixtures(leagueId, seasonId) ?? [])
						.Where(f => f.Date <= today)
						.Where(f => f is CompletedFixture or PostponedFixture)
						.OrderByDescending(f => f.Date)
						.ThenBy(f => f.Division)
						.Take((int)NoOfFixtures)];
		foreach (Fixture fixture in fixtures) {
			if (fixture is CompletedFixture completedFixture) {
				bool homeWin = (completedFixture.ForHome > completedFixture.ForAway);
				bool awayWin = (completedFixture.ForHome < completedFixture.ForAway);
				string fullScore = $"{completedFixture.ForHome} - {completedFixture.ForAway}";
				_ = fixtureResults.TryAdd(completedFixture.Id, new(Id: completedFixture.Id, HomeWin: homeWin, AwayWin: awayWin, FullScore: fullScore));
			}
		}
	}

	private record FixtureResult(int Id, bool HomeWin, bool AwayWin, string FullScore);

	private static string PlayerToLink(MatchPlayer player, TT365LeagueId leagueId, TT365SeasonId seasonId)
		=> $"""PlayerSummary/{leagueId}/{seasonId}/{player.Name.Replace(" ", "_")}/{player.Id}""";

}
