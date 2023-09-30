namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal async Task<int?> GetId(string name, LookupType type, string leagueId, int competitionId)
	{
		string fileName = $"{leagueId}_{competitionId}_lookup_tables.json";

		List<LookupValue>? lookup = await GetLookupTables(leagueId, competitionId);

		return lookup
			?.FirstOrDefault(l => l.Type == type && l.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
			?.Id;
	}
}
