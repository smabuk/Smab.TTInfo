﻿namespace Smab.TTInfo.Cli;

[Description("TTInfo cli utility for reading Table Tennis 365.")]
internal sealed class TTInfoCliCommand : Command<TTInfoCliCommand.Settings>
{
	public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
	{
		int year = settings.Year ?? DateTime.Today.AddMonths(-8).Year;
		return settings.Year switch
		{
			null when settings.SearchPlayers is not null && settings.SearchOpponents is not null 
			  => TTInfoCli.PlayerVsPlayer(
					settings.TTInfoId,
					year,
					Path.GetFullPath(settings.CacheFolder),
					settings.SearchPlayers,
					settings.SearchOpponents).Result,
			_ => TTInfoCli.Run(settings.TTInfoId,
					year,
					Path.GetFullPath(settings.CacheFolder),
					settings.ShowTeamSearch,
					settings.SearchPlayers,
					settings.SearchOpponents).Result
		};
	}

	public sealed class Settings : CommandSettings
	{
		[Description("The ttinfo id. Which is the part of the uri representing the league web site.")]
		[CommandArgument(0, "<TTInfoId>")]
		public required string TTInfoId { get; init; }

		[Description("The year in which the season starts.")]
		[CommandArgument(1, "[Year]")]
		public int? Year { get; init; }


		[Description("Show the team members in the team specified by the search.")]
		[CommandOption("-t")]
		public string? ShowTeamSearch { get; init; } = null;

		[Description("Show match details for a player.")]
		[CommandOption("-p")]
		public string? SearchPlayers { get; init; } = null;

		[Description("Limit match details to these opponents.")]
		[CommandOption("--vs")]
		public string? SearchOpponents { get; init; } = null;

		[Description("Cache folder used for storing the html and json files so that excessive server calls can be avoided.")]
		[CommandOption("-c|--cacheFolder")]
		public string CacheFolder { get; init; } = Path.Combine(Path.GetTempPath(), "tt365", "cache");
	}
}
