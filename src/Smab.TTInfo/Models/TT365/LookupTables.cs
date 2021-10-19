namespace Smab.TTInfo.Models.TT365;

public record LookupTables
{
	public List<IdNamePair> DivisionLookup = new();
	public List<IdNamePair> ClubLookup = new();
	public List<IdNamePair> TeamLookup = new();
	public List<IdNamePair> VenueLookup = new();

}
