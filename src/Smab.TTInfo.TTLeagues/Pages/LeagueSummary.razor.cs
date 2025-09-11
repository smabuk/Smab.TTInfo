namespace Smab.TTInfo.TTLeagues.Pages;

public partial class LeagueSummary
{
	[EditorRequired]
	[Parameter]
	public string TTInfoId { get; set; } = "";

	[EditorRequired]
	[Parameter]
	public int? CompetitionId { get; set; }

	private string LeagueName { get; set; } = "";
	private League? League;
	private string currentCompetition = "";
	private List<String> competitionNames = [];
	private readonly Dictionary<int, List<Division>> divisionsList = [];

	protected override async Task OnParametersSetAsync()
	{
		if (League is null) {
			LeagueName = $"{TTInfoId} League";
			League = await _ttleagues.GetLeague(TTInfoId);
			LeagueName = @League?.TenantsHost?.Name ?? "";

			competitionNames = League?.CurrentCompetitions.Select(s => s.Name).OrderByDescending(s => s).ToList() ?? [];
			if (CompetitionId is null) {
				if (League?.CurrentCompetitions.Any() ?? false) {
					foreach (Competition competition in League.CurrentCompetitions) {
						List<Division>? divisions = await _ttleagues.GetDivisions(TTInfoId, competition.Id);
						_ = divisionsList.TryAdd(competition.Id, divisions);
					}
				}
			} else {
				List<Division>? divisions = await _ttleagues.GetDivisions(TTInfoId, (int)CompetitionId);
				_ = divisionsList.TryAdd((int)CompetitionId, divisions);
			}
		}

		if (CompetitionId is not null) {
			currentCompetition = League?.CurrentCompetitions.Single(s => s.Id == CompetitionId).Name ?? "";
		} else {
			CompetitionId = League?.CurrentCompetitions.FirstOrDefault()?.Id;
		}
	}

}
