namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents the options for configuring the view of fixtures in a sports management system.
/// </summary>
/// <remarks>This record provides various properties to customize the display and filtering of fixtures, such as
/// season, division, club, team, venue, and additional view preferences.</remarks>
public record FixturesViewOptions
{
	public string Season { get; set; } = "";
	public string DivisionName { get; set; } = "All Divisions";
	public string ClubId { get; set; } = "";
	public string TeamId { get; set; } = "";
	public string VenueId { get; set; } = "";
	public int ViewModeType { get; set; } = FixturesViewType.Advanced;
	public bool HideCompletedFixtures { get; set; } = false;
	public bool MergeDivisions { get; set; } = true;
	public bool ShowByWeekNo { get; set; } = true;
}
