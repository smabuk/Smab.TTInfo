﻿namespace Smab.TTInfo.TT365.Models;

public record Division(string Id, string Name)
{
	public List<Team> Teams { get; set; } = new();
	public int TeamCount => Teams?.Count ?? 0;
}