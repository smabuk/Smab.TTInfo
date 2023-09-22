namespace Smab.TTInfo.TT365.Models;

public record LookupTables
{
	public List<IdNamePair> DivisionLookup = new();
	public List<IdNamePair> ClubLookup = new();
	public List<IdNamePair> TeamLookup = new();
	public List<IdNamePair> VenueLookup = new();
}
