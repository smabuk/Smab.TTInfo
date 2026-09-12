namespace Smab.TTInfo.TT365.Pages;
public partial class LeagueSummary
{
	[EditorRequired] [Parameter] public string LeagueId { get; set; } = "";
	[EditorRequired] [Parameter] public string? SeasonId { get; set; } = null;

	private string LeagueName { get; set; } = "";
	private League? League { get; set; }

	private TT365LeagueId leagueId;
	private List<Division> divisions = [];
	private bool isLoading = false;

	protected override async Task OnParametersSetAsync()
	{
		isLoading = true;

		if (LeagueId is not null) {
			leagueId = (TT365LeagueId)LeagueId;
			LeagueName = LeagueId;
			League = await _tt365.GetLeague((TT365LeagueId)LeagueId);
			if (League is null) { return; }

			LeagueName = League.Name;
			SeasonId = string.IsNullOrWhiteSpace(SeasonId) ? League.CurrentSeason.Name ?? "" : SeasonId;
			if (SeasonId != League.CurrentSeason.Name) {
				divisions = [.. League.Seasons.FirstOrDefault(s => s.Name == SeasonId)?.Divisions ?? []];
			} else {
				divisions = [.. League.CurrentSeason.Divisions];
			}
		}

		isLoading = false;
	}
}
