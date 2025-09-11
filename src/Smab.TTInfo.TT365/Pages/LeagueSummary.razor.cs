namespace Smab.TTInfo.TT365.Pages;
public partial class LeagueSummary
{
	[EditorRequired]
	[Parameter]
	public string LeagueId { get; set; } = "";

	private string LeagueName { get; set; } = "";
	private League? League { get; set; }

	private string currentSeason = "";
	private List<string> seasons = [];

	protected override async Task OnParametersSetAsync()
	{
		if (LeagueId is not null) {
			LeagueName = LeagueId;
			League = await _tt365.GetLeague((TT365LeagueId)LeagueId);
			LeagueName = League?.Name ?? "";
			currentSeason = League?.CurrentSeason.Name ?? "";
			seasons = League?.Seasons.Select(s => s.Name).Append(currentSeason).OrderByDescending(s => s).ToList() ?? [];
		}
	}
}
