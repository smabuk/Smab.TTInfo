using Microsoft.AspNetCore.Components;

namespace Smab.TTInfo.TT365.Pages;

/// <summary>
/// Represents a summary of a division within a league and season.
/// </summary>
/// <remarks>This class is used to encapsulate information about a specific division, including its identifiers,
/// name, and associated data. It provides functionality to load division details asynchronously based on the provided
/// parameters.</remarks>
public partial class DivisionSummary(ITT365Reader _tt365, NavigationManager _navManager)
{
	[EditorRequired]
	[Parameter]
	public string LeagueId { get; set; } = "";

	[EditorRequired]
	[Parameter]
	public string SeasonId { get; set; } = "";

	[Parameter]
	public string DivisionId { get; set; } = "";

	[Parameter]
	public string DivisionName { get; set; } = "";

	[Parameter]
	public Division? Division { get; set; } = null;

	private bool isLoading = true;

	protected override async Task OnParametersSetAsync()
	{
		isLoading = true;

		Division ??= await LoadDivision();

		isLoading = false;
	}

	private async Task<Division?> LoadDivision()
	{
		if (string.IsNullOrEmpty(LeagueId) || string.IsNullOrEmpty(SeasonId)) {
			return null;
		}

		if (string.IsNullOrEmpty(DivisionId) && string.IsNullOrEmpty(DivisionName)) {
			return null;
		}

		return (await _tt365.GetDivisions((TT365LeagueId)LeagueId, (TT365SeasonId)SeasonId))
			.FirstOrDefault(d => d.Id == DivisionId || d.Name == DivisionName);
	}

	private bool IsPage() => _navManager.Uri.Contains($"DivisionSummary");

}
