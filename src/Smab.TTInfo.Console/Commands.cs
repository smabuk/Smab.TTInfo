namespace Smab.TTInfo.Console;

/// <summary>
/// Represents a command-line utility for interacting with Table Tennis 365 (TT365) data.
/// </summary>
/// <remarks>This command is used to retrieve and process data from the TT365 platform, such as player match
/// details, team information, and other league-related data. It supports various options for filtering and customizing
/// the output, including specifying a season year, searching for specific players or opponents, and caching data to
/// reduce server calls.</remarks>

[Description("TTInfo cli utility for reading Table Tennis 365.")]
internal sealed class TTInfoCliCommand : Command<TTInfoCliCommand.Settings>
{
	public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings, CancellationToken cancellationToken)
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

	/// <summary>
	/// Represents the settings for a command that interacts with a league website, allowing users to specify various
	/// options such as the league identifier, season year, and search filters.
	/// </summary>
	/// <remarks>This class provides configuration options for commands, including specifying the league identifier,
	/// filtering by season year, searching for team members or players, and limiting match details. It also supports
	/// caching to reduce server calls.</remarks>
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
