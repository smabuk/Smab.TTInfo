namespace Smab.TTInfo.TT365.Models;

public record LookupTables
{
	public List<IdNamePair> DivisionLookup = [];
	public List<IdNamePair> ClubLookup = [];
	public List<IdNamePair> TeamLookup = [];
	public List<IdNamePair> VenueLookup = [];
}
