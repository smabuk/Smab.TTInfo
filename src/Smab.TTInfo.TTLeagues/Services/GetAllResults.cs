namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<Fixtures?> GetAllResults(string ttinfoId, int? competitionId = null)
	{
		string fileName = competitionId is null
			? $"{ttinfoId}_results.json"
			: $"{ttinfoId}_{competitionId}_results.json";

		return await LoadJsonAsync<Fixtures>(
			ttinfoId,
			$"matches/?competitionId={competitionId}&type=2",
			fileName);
	}
}
