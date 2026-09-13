using Smab.TTInfo.Shared.Helpers;

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

	public League? League { get; set; }

	private bool isLoading = true;
	private TT365SeasonId seasonId;
	private TT365LeagueId leagueId;

	protected override async Task OnParametersSetAsync()
	{
		isLoading = true;

		leagueId = (TT365LeagueId)LeagueId;
		seasonId = (TT365SeasonId)SeasonId;
		League ??= await _tt365.GetLeague(leagueId);
		if (League is null) {
			isLoading = false;
			return;
		}

		Division ??= League.GetDivisionByIdOrName(seasonId, DivisionId, DivisionName);

		isLoading = false;
	}

	private bool IsPage() => _navManager.IsPage(nameof(DivisionSummary));

}
