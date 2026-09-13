namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents a collection of lookup tables for various entities such as divisions, clubs, teams, and venues.
/// </summary>
/// <remarks>This record provides pre-populated lookup data for use in scenarios where entity identifiers need to
/// be mapped to their corresponding names. Each lookup table is represented as a list of <see cref="IdNamePair"/>
/// objects.</remarks>
public record LookupTables
{
	public ImmutableList<IdNamePair> DivisionLookup { get; set; } = [];
	public ImmutableList<IdNamePair> ClubLookup { get; set; } = [];
	public ImmutableList<IdNamePair> TeamLookup { get; set; } = [];
	public ImmutableList<IdNamePair> VenueLookup { get; set; } = [];
}

public static class LookupTableExtensions
{
	extension(LookupTables lookupTables)
	{
		public IdNamePair? GetTeamByName(string teamName)
			=> lookupTables.TeamLookup.FirstOrDefault(t => t.Name.Equals(teamName.Replace("%20", " ").Replace("_", " "), StringComparison.OrdinalIgnoreCase));
	}
}

