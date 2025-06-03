namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents a collection of lookup tables for various entities such as divisions, clubs, teams, and venues.
/// </summary>
/// <remarks>This record provides pre-populated lookup data for use in scenarios where entity identifiers need to
/// be mapped to their corresponding names. Each lookup table is represented as a list of <see cref="IdNamePair"/>
/// objects.</remarks>
public record LookupTables
{
	public ImmutableList<IdNamePair> DivisionLookup = [];
	public ImmutableList<IdNamePair> ClubLookup = [];
	public ImmutableList<IdNamePair> TeamLookup = [];
	public ImmutableList<IdNamePair> VenueLookup = [];
}
