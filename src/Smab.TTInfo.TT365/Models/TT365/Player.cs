namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents a player with associated statistics, rankings, and performance data.
/// </summary>
/// <remarks>
/// This class provides properties to access a player's details, including their name, performance metrics, rankings at
/// various levels, and a collection of their results. It also includes derived properties for extracting the player's
/// unique identifier and season identifier from the player's URL.
/// </remarks>
[DebuggerDisplay("Name: {DisplayName,nq}")]
public record class Player(string Name, int PlayerId, bool IsSubstitute = false)
{
	public string PlayerURL { get; set; } = "";
	public int Played { get; set; }
	public int Won { get; set; }
	public float WinPercentage { get; set; }
	public string PoMAwards { get; set; } = "";
	public string Form { get; set; } = "";
	public int ClubRanking { get; set; }
	public int LeagueRanking { get; set; }
	public int CountyRanking { get; set; }
	public int RegionalRanking { get; set; }
	public int NationalRanking { get; set; }
	public int Id => PlayerId == 0
		? string.IsNullOrWhiteSpace(PlayerURL)
			? 0
			: int.TryParse(PlayerURL.Split(['/', '=']).LastOrDefault() ?? "", out int id)
				? id
				: 0
		: PlayerId;
	public TT365SeasonId SeasonId
		=> PlayerURL.Split('/').Skip(4).FirstOrDefault() is string seasonId && !string.IsNullOrWhiteSpace(seasonId)
		? new TT365SeasonId(seasonId)
		: default;

	public ImmutableList<PlayerResult> PlayerResults { get; set; } = [];

	public string DisplayName => IsSubstitute ? $"{Name} (sub)" : Name;
}

public static class PlayerExtensions
{
	extension(Player)
	{
		public static Player EmptyPlayer => new("", 0, false);
	}
}
