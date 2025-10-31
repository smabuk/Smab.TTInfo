namespace Smab.TTInfo.TT365.Models.TT365;


/// <summary>
/// Provides extension methods for converting a <see cref="Fixture"/> to its derived types representing different fixture states.
/// </summary>
public static partial class FixtureExtensions
{
	/// <summary>
	/// Converts a <see cref="Fixture"/> to a <see cref="CompletedFixture"/>.
	/// </summary>
	/// <param name="fixture">The fixture to convert.</param>
	/// <returns>A new <see cref="CompletedFixture"/> instance with properties copied from the original fixture.</returns>
	public static CompletedFixture ToCompleted(this Fixture fixture)
	{
		return new CompletedFixture(
			fixture.Division,
			fixture.Description,
			fixture.Date,
			fixture.HomeTeam,
			fixture.AwayTeam,
			fixture.Venue);
	}

	/// <summary>
	/// Converts a <see cref="Fixture"/> to a <see cref="PostponedFixture"/>.
	/// </summary>
	/// <param name="fixture">The fixture to convert.</param>
	/// <returns>A new <see cref="PostponedFixture"/> instance with properties copied from the original fixture.</returns>
	public static PostponedFixture ToPostponed(this Fixture fixture, string reason)
	{
		return new PostponedFixture(
			fixture.Division,
			fixture.Description,
			fixture.Date,
			fixture.HomeTeam,
			fixture.AwayTeam,
			fixture.Venue,
			reason);
	}

	/// <summary>
	/// Converts a <see cref="Fixture"/> to a <see cref="RearrangedFixture"/>.
	/// </summary>
	/// <param name="fixture">The fixture to convert.</param>
	/// <returns>A new <see cref="RearrangedFixture"/> instance with properties copied from the original fixture.</returns>
	public static RearrangedFixture ToRearranged(this Fixture fixture, DateOnly originalDate, string reason)
	{
		return new RearrangedFixture(
			fixture.Division,
			fixture.Description,
			fixture.Date,
			fixture.HomeTeam,
			fixture.AwayTeam,
			fixture.Venue,
			originalDate,
			reason
		);
	}

	/// <summary>
	/// Converts a <see cref="Fixture"/> to a <see cref="VoidFixture"/>.
	/// </summary>
	/// <param name="fixture">The fixture to convert.</param>
	/// <returns>A new <see cref="VoidFixture"/> instance with properties copied from the original fixture.</returns>
	public static VoidFixture ToVoid(this Fixture fixture, string reason)
	{
		return new VoidFixture(
			fixture.Division,
			fixture.Description,
			fixture.Date,
			fixture.HomeTeam,
			fixture.AwayTeam,
			fixture.Venue,
			reason
		);
	}

	public static TimeOnly Time(this Fixture fixture)
	{
		TimeOnly defaultTime = TT365Reader.DEFAULT_START_TIME; // 7:30pm

		if (fixture.Venue.Contains("CURZON", StringComparison.OrdinalIgnoreCase)
			|| fixture.Venue.Contains("RBL", StringComparison.OrdinalIgnoreCase)) // 7pm start time
		{
			return defaultTime.AddMinutes(-30);
		}

		if (fixture.Venue.Contains("BRAYBROOKE", StringComparison.OrdinalIgnoreCase)) // 7:15pm start time
		{
			return defaultTime.AddMinutes(-15);
		}

		return defaultTime;
	}

	public static bool HasDefaultTime(this Fixture fixture) => fixture.Time() == TT365Reader.DEFAULT_START_TIME;
}

