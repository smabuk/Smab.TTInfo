﻿namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents a fixture that does not produce a result, typically due to a specific reason.
/// </summary>
/// <remarks>This type is used to indicate scenarios where a fixture is void, such as when a match or event is
/// canceled or invalidated. The <see cref="Reason"/> property provides additional context about why the fixture is
/// void.</remarks>
public record VoidFixture(
	string Division,
	string Description,
	DateOnly Date,
	string HomeTeam,
	string AwayTeam,
	string Venue,
	string Reason = ""
) : Fixture(Division, Description, Date, HomeTeam, AwayTeam, Venue);
