namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	/// <summary>
	/// Retrieves the identifier (ID) of a lookup value that matches the specified criteria.
	/// </summary>
	/// <remarks>This method queries a lookup table and searches for a value that matches the specified name and
	/// type.</remarks>
	/// <param name="name">The name of the lookup value to search for. The comparison is case-insensitive.</param>
	/// <param name="type">The type of the lookup value to search for.</param>
	/// <param name="ttinfoId">The identifier of the lookup table to query.</param>
	/// <param name="competitionId">The identifier of the competition associated with the lookup.</param>
	/// <returns>The ID of the matching lookup value if found; otherwise, <see langword="null"/>.</returns>
	internal async Task<int?> GetId(string name, LookupType type, string ttinfoId, int competitionId)
	{
		List<LookupValue>? lookup = await GetLookupTables(ttinfoId);

		return lookup
			?.FirstOrDefault(l => l.Type == type && l.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
			?.Id;
	}
}
