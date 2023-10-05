namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<List<Ranking>> GetRankings(string ttinfoId)
	{
		string fileName = $"{ttinfoId}_rankings.json";

		return await LoadJsonAsync<List<Ranking>>(
			ttinfoId,
			$"rankings",
			fileName) ?? [];
	}
}
