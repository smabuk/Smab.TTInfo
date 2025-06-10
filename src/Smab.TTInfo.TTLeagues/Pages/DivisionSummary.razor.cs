using Microsoft.AspNetCore.Components;

using Smab.TTInfo.Shared.Helpers;

namespace Smab.TTInfo.TTLeagues.Pages;

/// <summary>
/// Represents a summary of a division within a league and season.
/// </summary>
/// <remarks>This class is used to encapsulate information about a specific division, including its identifiers,
/// name, and associated data. It provides functionality to load division details asynchronously based on the provided
/// parameters.</remarks>
public partial class DivisionSummary(TTLeaguesReader _ttleagues, NavigationManager _navManager)
{
	[EditorRequired]
	[Parameter]
	public string TTInfoId { get; set; } = "";

	[EditorRequired]
	[Parameter]
	public int CompetitionId { get; set; }

	[EditorRequired]
	[Parameter]
	public int DivisionId { get; set; }

	private Division? division;
	private List<TeamStanding> teamStandings = [];

	private bool isLoading = true;

	protected override async Task OnParametersSetAsync()
	{
		isLoading = true;

		if (string.IsNullOrEmpty(TTInfoId) || CompetitionId <= 0 || DivisionId <= 0) {
			isLoading = false;
			return;
		}

		division ??= (await _ttleagues.GetDivisions(TTInfoId, CompetitionId))
			.FirstOrDefault(d => d.Id == DivisionId);
		teamStandings = await _ttleagues.GetDivisionStandings(DivisionId, TTInfoId) ?? [];

		isLoading = false;
	}

	private bool IsPage() => _navManager.IsPage(nameof(DivisionSummary));
}
