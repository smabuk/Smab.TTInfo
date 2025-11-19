namespace Smab.TTInfo.TTLeagues.Pages;
public partial class MatchPage
{

	[EditorRequired]
	[Parameter]
	public string TTInfoId { get; set; } = "";

	[EditorRequired]
	[Parameter]
	public int MatchId { get; set; }

	private MatchCard? matchCard;
	private Division? division;

	protected override async Task OnParametersSetAsync()
	{
		matchCard ??= await _ttleagues.GetMatch(MatchId, TTInfoId);
		if (matchCard is not null) {
			division ??= (await _ttleagues.GetDivisions(TTInfoId, matchCard.Match.CompetitionId)).FirstOrDefault(d => d.Id == matchCard?.Match.DivisionId);
		}
	}

	private static string HomeAwayOrDraw(MatchSet set)
	{
		return (set.Completed is null) || (set.HomeScore == 0 && set.AwayScore == 0)
			? ""
			: set.HomeScore > set.AwayScore
		? "Home"
		: set.AwayScore > set.HomeScore
			? "Away"
			: "Draw";
	}

	private static string DisplayName(MatchPlayer player)
	{
		if (player.Forfeit is not null) {
			return "Forfeit";
		}

		if (player.Scratch is not null) {
			return $"{player.PrintoutName} (scratched)";
		}

		return player.PrintoutName;
	}

}
