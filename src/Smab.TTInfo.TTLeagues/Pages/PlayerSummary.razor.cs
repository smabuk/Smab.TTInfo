using Microsoft.AspNetCore.Components;

namespace Smab.TTInfo.TTLeagues.Pages;
public partial class PlayerSummary
{
	[EditorRequired]
	[Parameter]
	public required string PlayerId { get; set; }

	[EditorRequired]
	[Parameter]
	public string PlayerName { get; set; } = "";

	[EditorRequired]
	[Parameter]
	public string TTInfoId { get; set; } = "";

	private List<PlayerStats> playerStatsList = [];
	private readonly Dictionary<int, string> divisionNames = [];
	private League? league;
	List<LookupValue> lookup = [];

	protected override async Task OnParametersSetAsync()
	{
		playerStatsList = [];
		StateHasChanged();
		league = await _ttleagues.GetLeague(TTInfoId);
		lookup = await _ttleagues.GetLookupTables(TTInfoId);

		if (league is not null) {
			foreach (int competitionId in league.CurrentCompetitions.Select(c => c.Id).Where(id => id is > 0)) {
				PlayerStats? playerStats = await _ttleagues.GetPlayerStats(PlayerId, TTInfoId, competitionId);
				if (playerStats is not null) {
					playerStatsList.Add(playerStats);
				}
			}
		}

		StateHasChanged();
	}

	string CalculateDivisionName(string name, int id)
	{
		if (String.IsNullOrWhiteSpace(name) || String.Equals(name, "no division", StringComparison.InvariantCultureIgnoreCase)) {
			if (divisionNames.TryGetValue(id, out string? value)) {
				name = value;
			} else {
				name = lookup
					.Where(l => l.Type == LookupType.Division && l.Id == id)
					.Select(l => l.Name)
					.First();
				divisionNames.Add(id, name);
			}
		}

		return name.Replace("division", "", StringComparison.InvariantCultureIgnoreCase).Trim();
	}

	static string CalculateMatchResult(int scoreFor, int scoreAgainst)
	{
		return (scoreFor - scoreAgainst) switch
		{
			> 0 => "win",
			< 0 => "loss",
			0 => "draw",
		};
	}

}
