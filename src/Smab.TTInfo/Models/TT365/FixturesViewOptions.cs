﻿namespace Smab.TTInfo.Models.TT365;

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
