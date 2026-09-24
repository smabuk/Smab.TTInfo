namespace Smab.TTInfo.TT365.Models.TT365;


/// <summary>
/// Provides extension methods for converting a <see cref="Fixture"/> to its derived types representing different fixture states.
/// </summary>
public static partial class FixtureExtensions
{
	extension(Fixture fixture)
	{
		/// <summary>
		/// Converts a <see cref="Fixture"/> to a <see cref="CompletedFixture"/>.
		/// </summary>
		/// <param name="fixture">The fixture to convert.</param>
		/// <returns>A new <see cref="CompletedFixture"/> instance with properties copied from the original fixture.</returns>
		public CompletedFixture ToCompleted()
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
		public PostponedFixture ToPostponed(string reason)
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
		public RearrangedFixture ToRearranged(DateOnly originalDate, string reason)
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
		public VoidFixture ToVoid(string reason)
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

		public TimeOnly Time()
		{
			TimeOnly defaultTime = TT365Reader.DEFAULT_START_TIME; // 7:30pm

			return fixture.Venue switch
			{
				string v when v.Contains("RBL", StringComparison.OrdinalIgnoreCase) => defaultTime.AddMinutes(-30), // 7:00pm start time
				string v when v.Contains("CURZON", StringComparison.OrdinalIgnoreCase) => defaultTime.AddMinutes(-30), // 7:00pm start time
				string v when v.Contains("BRAYBROOKE", StringComparison.OrdinalIgnoreCase) => defaultTime.AddMinutes(-15), // 7:15pm start time
				_ => defaultTime
			};
		}

		public bool HasDefaultTime() => fixture.Time() == TT365Reader.DEFAULT_START_TIME;
	}
}

