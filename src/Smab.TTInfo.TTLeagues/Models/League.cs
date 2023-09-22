namespace Smab.TTInfo.TTLeagues.Models;

internal sealed class League
{
	public required string Id { get; set; }
	public required long CurrentCompetitionId { get; set; }
	public required TenantsHost? TenantsHost { get; set; }
	public required WebsitesHost? WebsitesHost { get; set; }
	public required Competition CurrentCompetition { get; set; }
	public required List<Competition> Competitions { get; set; }
}

