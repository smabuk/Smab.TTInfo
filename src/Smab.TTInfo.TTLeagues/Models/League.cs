namespace Smab.TTInfo.TTLeagues.Models;

internal sealed class League
{
	public required string Id { get; set; }
	//public required long CurrentCompetitionId { get; set; }
	public required TenantsHost? TenantsHost { get; set; }
	public required WebsitesHost? WebsitesHost { get; set; }
	public required List<Competition> CurrentCompetitions { get; set; }
	public required List<Competition> ArchivedCompetitions { get; set; }
}

