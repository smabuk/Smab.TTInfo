namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents the options for configuring the view of fixtures in a sports management system.
/// </summary>
/// <remarks>This record provides various properties to customize the display and filtering of fixtures, such as
/// season, division, club, team, venue, and additional view preferences.</remarks>
public record FixturesViewOptions(
	string Season,
	string DivisionName,
	string ClubId,
	string TeamId,
	string VenueId,
	FixturesViewType ViewModeType,
	bool HideCompletedFixtures,
	bool MergeDivisions,
	bool ShowByWeekNo
)
{
	public static FixturesViewOptions Create(
		string season = "",
		string divisionName = "All Divisions",
		string clubId = "",
		string teamId = "",
		string venueId = "",
		FixturesViewType viewModeType = FixturesViewType.Advanced,
		bool hideCompletedFixtures = false,
		bool mergeDivisions = true,
		bool showByWeekNo = true
	)
	{
		return new FixturesViewOptions(
			season,
			divisionName,
			clubId,
			teamId,
			venueId,
			viewModeType,
			hideCompletedFixtures,
			mergeDivisions,
			showByWeekNo
		);
	}
}
