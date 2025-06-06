using Microsoft.AspNetCore.Components;

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
	private readonly Dictionary<int, List<TeamStanding>> teamStandingsList = [];

	private readonly List<Division> divisions = [];

	protected override async Task OnParametersSetAsync()
	{
		if (League is null) {
			LeagueName = $"{TTInfoId} League";
			League = await _ttleagues.GetLeague(TTInfoId);
			LeagueName = @League?.TenantsHost?.Name ?? "";
			StateHasChanged();
			competitionNames = League?.CurrentCompetitions.Select(s => s.Name).OrderByDescending(s => s).ToList() ?? [];
			if (CompetitionId is null) {
				if (League?.CurrentCompetitions.Any() ?? false) {
					foreach (Competition competition in League.CurrentCompetitions) {
						List<Division>? divisions = await _ttleagues.GetDivisions(TTInfoId, competition.Id);
						_ = divisionsList.TryAdd(competition.Id, divisions);
						StateHasChanged();
					}
				}
			} else {
				List<Division>? divisions = await _ttleagues.GetDivisions(TTInfoId, (int)CompetitionId);
				_ = divisionsList.TryAdd((int)CompetitionId, divisions);
			}

			foreach (List<Division> divisions in divisionsList.Values) {
				foreach (Division division in divisions) {
					List<TeamStanding>? teamStandings = await _ttleagues.GetDivisionStandings(division.Id, TTInfoId);
					if (teamStandings is not null) {
						_ = teamStandingsList.TryAdd(division.Id, teamStandings);
					}
				}

				StateHasChanged();
			}
		}

		if (CompetitionId is not null) {
			currentCompetition = League?.CurrentCompetitions.Single(s => s.Id == CompetitionId).Name ?? "";
		}
	}

}
