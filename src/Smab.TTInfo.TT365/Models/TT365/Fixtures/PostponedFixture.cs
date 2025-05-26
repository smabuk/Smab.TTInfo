namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents a fixture that has been postponed, including the reason for the postponement.
/// </summary>
/// <remarks>This record extends the <see cref="Fixture"/> type to include additional information about why the
/// fixture was postponed.</remarks>
public record PostponedFixture(
	string Division,
	string Description,
	DateOnly Date,
	string HomeTeam,
	string AwayTeam,
	string Venue,
	string Reason = ""
) : Fixture(Division, Description, Date, HomeTeam, AwayTeam, Venue);
