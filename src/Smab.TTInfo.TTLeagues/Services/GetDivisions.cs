namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<List<Division>> GetDivisions(string ttinfoId, int competitionId)
	{
		string fileName = $"{ttinfoId}_{competitionId}_divisions.json";

		return await LoadJsonAsync<List<Division>>(
			ttinfoId,
			$"competitions/{competitionId}/divisions",
			fileName) ?? [];
	}
}

