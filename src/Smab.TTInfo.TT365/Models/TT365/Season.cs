namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents a season, including its unique identifier, name, associated lookup tables, and divisions.
/// </summary>
/// <remarks>A season is a logical grouping that contains divisions and associated lookup data. Use this record to
/// manage and organize season-specific information.</remarks>
/// <param name="Id"></param>
/// <param name="Name"></param>
public record Season(
	TT365SeasonId Id,
	string Name,
	LookupTables Lookups,
	ImmutableList<Division> Divisions
)
{
	public int DivisionCount => Divisions.Count;
}
