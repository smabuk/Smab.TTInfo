﻿namespace Smab.TTInfo;

public partial class TT365Reader
{
	public string CsvFromFixtures(ICollection<Fixture> Fixtures)
	{
		string output = "";

		foreach (Fixture fixture in Fixtures)
		{
			output += $"{fixture.Date:dd/MM/yyyy},{fixture.HomeTeam},{fixture.AwayTeam},{fixture.Venue}{Environment.NewLine}";
		}

		return output;
	}
}

