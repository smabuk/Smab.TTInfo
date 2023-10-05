namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<Fixtures?> GetAllFixtures(string ttinfoId, int? competitionId = null)
	{
		string fileName = competitionId is null
			? $"{ttinfoId}_fixtures.json"
			: $"{ttinfoId}_{competitionId}_fixtures.json";

		return await LoadJsonAsync<Fixtures>(
			ttinfoId,
			$"matches/?competitionId={competitionId}&type=1",
			fileName);
	}
}
