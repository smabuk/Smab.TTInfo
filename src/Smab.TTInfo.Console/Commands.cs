﻿namespace Smab.TTInfo.Cli;

[Description("TTInfo cli utility for reading Table Tennis 365.")]
internal sealed class TTInfoCliCommand : Command<TTInfoCliCommand.Settings>
{

	public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
	{
		TTInfoCli.Run(settings.LeagueId, settings.Year, settings.CacheFolder, settings.ShowTeamSearch, settings.SearchPlayers).Wait();
		return 0;
	}

	public sealed class Settings : CommandSettings
	{
		[Description("The league id. Which is the part of the uri representing the league web site.")]
		[CommandArgument(0, "<leagueId>")]
		public string LeagueId { get; init; } = "";

		[Description("The year in which the season starts.")]
		[CommandArgument(1, "[Year]")]
		public int Year { get; init; } = DateTime.Today.AddMonths(-8).Year;


		[Description("Show the team members in the team specified by the search.")]
		[CommandOption("-t")]
		public string? ShowTeamSearch { get; init; } = null;

		[Description("Show match details for a player.")]
		[CommandOption("-p")]
		public string? SearchPlayers { get; init; } = null;

		[Description("Cache folder used for storing the html and json files so the server calls can be avoided.")]
		[CommandOption("-c|--cacheFolder")]
		public string CacheFolder { get; init; } = Path.Combine(Path.GetTempPath() , "tt365", "cache");
	}
}
