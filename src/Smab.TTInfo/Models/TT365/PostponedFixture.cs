﻿namespace Smab.TTInfo.Models.TT365;

public record PostponedFixture : Fixture
{
	public string Reason { get; set; } = "";
}
