namespace Smab.TTInfo.TT365.Pages;
public partial class MatchPage
{

	[EditorRequired]
	[Parameter]
	public string LeagueId { get; set; } = "";

	[EditorRequired]
	[Parameter]
	public string SeasonId { get; set; } = "";

	[EditorRequired]
	[Parameter]
	public int MatchId { get; set; }

	private TT365LeagueId leagueId;
	private TT365SeasonId seasonId;
	private League? league;
	private MatchCard? matchCard;

	protected override async Task OnParametersSetAsync()
	{
		leagueId = (TT365LeagueId)LeagueId;
		league = await _tt365.GetLeague(leagueId);
		if (league is null) {
			return;
		}

		seasonId = string.IsNullOrWhiteSpace(SeasonId) ? league.CurrentSeasonId : (TT365SeasonId)SeasonId;
		SeasonId = seasonId.ToString();

		matchCard ??= await _tt365.GetMatchCard(leagueId, MatchId, seasonId);
		//if (matchCard is not null) {
		//	division ??= (await _tt365.GetDivisions(TTInfoId, matchCard.Match.CompetitionId)).FirstOrDefault(d => d.Id == matchCard?.Match.DivisionId);
		//}
	}

	//private static string HomeAwayOrDraw(MatchSet set)
	//{
	//	return (set.Completed is null) || (set.HomeScore == 0 && set.AwayScore == 0)
	//		? ""
	//		: set.HomeScore > set.AwayScore
	//	? "Home"
	//	: set.AwayScore > set.HomeScore
	//		? "Away"
	//		: "Draw";
	//}

	//private static string DisplayName(MatchPlayer player)
	//{
	//	if (player.Forfeit is not null) {
	//		return "Forfeit";
	//	}

	//	if (player.Scratch is not null) {
	//		return $"{player.PrintoutName} (scratched)";
	//	}

	//	return player.PrintoutName;
	//}

}
