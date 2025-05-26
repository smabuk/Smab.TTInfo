namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
	/// <summary>
	/// Converts a collection of fixtures into a CSV-formatted string.
	/// </summary>
	/// <remarks>The date field in the CSV output is formatted as "dd/MM/yyyy".</remarks>
	/// <param name="fixtures">A collection of <see cref="Fixture"/> objects to be converted. Cannot be null.</param>
	/// <returns>A CSV-formatted string where each line represents a fixture, with fields for the date, home team, away team, and
	/// venue, separated by commas. Each line ends with a newline character.</returns>
	public string CsvFromFixtures(ICollection<Fixture> fixtures)
	{
		System.Text.StringBuilder output = new();

		foreach (Fixture fixture in fixtures) {
			_ = output.Append($"{fixture.Date:dd/MM/yyyy},{fixture.HomeTeam},{fixture.AwayTeam},{fixture.Venue}{Environment.NewLine}");
		}

		return output.ToString();
	}
}

