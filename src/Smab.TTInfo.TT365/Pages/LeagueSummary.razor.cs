namespace Smab.TTInfo.TT365.Pages;
public partial class LeagueSummary
{
	[EditorRequired] [Parameter] public string LeagueId { get; set; }
	[EditorRequired] [Parameter] public string? SeasonId { get; set; }

	private League? League { get; set; }
	private Season? Season { get; set; }

	private TT365LeagueId leagueId;
	private TT365SeasonId seasonId;
	private bool isLoading = false;

	protected override async Task OnParametersSetAsync()
	{
		isLoading = true;

		if (LeagueId is not null) {
			leagueId = (TT365LeagueId)LeagueId;
			League = await _tt365.GetLeague(leagueId);
			if (League is null) { return; }

			seasonId = string.IsNullOrWhiteSpace(SeasonId) ? League.CurrentSeason.Id : (TT365SeasonId)SeasonId;
			Season = League.GetSeason(seasonId);
		}

		isLoading = false;
	}
}
