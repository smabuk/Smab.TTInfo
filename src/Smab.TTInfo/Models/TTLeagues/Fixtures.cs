﻿namespace Smab.TTInfo.Models.TTLeagues;

public partial class Fixtures
{
	public List<Group> Groups { get; set; } = new();
	public List<Match> Matches { get; set; } = new();
	public long Type { get; set; }
	public long Total { get; set; }
}
