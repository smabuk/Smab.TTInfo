namespace Smab.TTInfo.Models.TT365;

public record Season(string Id, string Name)
{
	public LookupTables Lookups { get; set; } = new ();
	public List<Division> Divisions { get; set; } = new();
	public int DivisionCount => Divisions?.Count ?? 0;
	
}
