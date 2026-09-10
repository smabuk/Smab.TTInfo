namespace Smab.TTInfo.TT365.Pages;

public partial class PlayerSummary
{
	[EditorRequired][Parameter] public int? PlayerId { get; set; } = null;
	[EditorRequired][Parameter] public string PlayerName { get; set; } = "";
	[EditorRequired][Parameter] public string SeasonName { get; set; } = "";
	[EditorRequired][Parameter] public string LeagueId { get; set; } = "";

	private bool isLoading = false;
	private League? league;
	private readonly Dictionary<string, List<PlayerResult>> playerResults = [];

	protected override async Task OnParametersSetAsync()
	{
		isLoading = true;
		PlayerName = PlayerName.Replace("_", " ");

		StateHasChanged();
		league = await _tt365.GetLeague((TT365LeagueId)LeagueId);
		if (league is null) {
			return;
		}

		if (PlayerId is not null) {
			TT365SeasonId seasonId = string.IsNullOrWhiteSpace(SeasonName) ? league.Seasons[0].Id : (TT365SeasonId)SeasonName;
			await UpdatePlayerStatsFromPreviousSeasonAsync(seasonId);
		} else {
			foreach (Season season in league.Seasons) {
				await UpdatePlayerStatsFromPreviousSeasonAsync(season.Id);
				StateHasChanged();
			}
		}

		isLoading = false;
	}

	private async Task UpdatePlayerStatsFromPreviousSeasonAsync(TT365SeasonId seasonId)
	{
		if (playerResults.ContainsKey(seasonId)) {
			return;
		}

		PlayerName = PlayerName.Replace("_", " ");

		Player playerStats = await _tt365.GetPlayerStatsByName((TT365LeagueId)LeagueId, PlayerName, seasonId) ?? new();
		if (playerStats is not null && playerStats.Id is not 0) {
			playerResults[seasonId] = [.. playerStats.PlayerResults];
		}
	}
}
