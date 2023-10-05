namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<Competition?> GetCompetition(string ttinfoId, int competitionId)
	{
		string fileName = $"{ttinfoId}_{competitionId}_competition.json";

		return await LoadJsonAsync<Competition>(
			ttinfoId,
			$"competitions/{competitionId}",
			fileName);
	}
}
