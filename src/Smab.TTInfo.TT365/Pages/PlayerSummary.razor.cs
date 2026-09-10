namespace Smab.TTInfo.TT365.Pages;
public partial class PlayerSummary
{
	[EditorRequired]
	[Parameter]
	public int PlayerId { get; set; }

	[EditorRequired]
	[Parameter]
	public string PlayerName { get; set; } = "";

	[EditorRequired]
	[Parameter]
	public string LeagueId { get; set; } = "";

	//private record FixtureResult(int Id, string Result, string FullScore);
	private bool isLoading = false;
	private List<PlayerResult> playerResults = [];
	private List<string> playerTeams = [];
	private League? league;
	private readonly Dictionary<string, List<PlayerResult>> previousPlayerResults = [];

	protected override async Task OnParametersSetAsync()
	{
		isLoading = true;
		PlayerName = PlayerName.Replace("_", " ");
		playerResults = [];
		StateHasChanged();
		league = await _tt365.GetLeague((TT365LeagueId)LeagueId);
		if (league is null) {
			return;
		}

		Player player = new()
		{
			Name = PlayerName,
			PlayerId = PlayerId,
		};

		Player? playerStats = await _tt365.GetPlayerStats((TT365LeagueId)LeagueId, player) ?? new();
		if (playerStats is not null) {
			playerResults = [.. playerStats.PlayerResults];
			playerTeams = [.. playerResults
			.GroupBy(p => p.PlayerTeamName)
			.OrderByDescending(g => g.Count())
			.Select(g => g.Key)];
			foreach (Season season in league.Seasons.Where(s => s.Id != league.GetCurrentSeason().Id)) {
				await UpdatePlayerStatsFromPreviousSeasonAsync(season.Id);
				StateHasChanged();
			}
		}

		isLoading = false;
	}

	private async Task UpdatePlayerStatsFromPreviousSeasonAsync(TT365SeasonId seasonId)
	{
		if (previousPlayerResults.ContainsKey(seasonId)) {
			return;
		}

		PlayerName = PlayerName.Replace("_", " ");

		Player playerStats = await _tt365.GetPlayerStatsByName((TT365LeagueId)LeagueId, PlayerName, seasonId) ?? new();
		if (playerStats is not null && playerStats.Id is not 0) {
			previousPlayerResults[seasonId] = [.. playerStats.PlayerResults];
		}
	}
}
