namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<int?> GetId(string name, LookupType type, string ttinfoId, int competitionId)
	{
		List<LookupValue>? lookup = await GetLookupTables(ttinfoId);

		return lookup
			?.FirstOrDefault(l => l.Type == type && l.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
			?.Id;
	}
}
