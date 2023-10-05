namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<List<TeamStanding>?> GetDivisionStandings(int divisionId, string ttinfoId)
	{
		string fileName = $"{ttinfoId}_division_{divisionId}_standings.json";

		return await LoadJsonAsync<List<TeamStanding>?>(
			ttinfoId,
			$"divisions/{divisionId}/standings",
			fileName);
	}
}
