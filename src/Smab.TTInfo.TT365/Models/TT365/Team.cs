namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents a sports team, including its details, players, fixtures, and results.
/// </summary>
/// <remarks>This class provides properties to store information about a team, such as its name, division,
/// captain, and performance statistics. It also includes collections for players, fixtures, and results,  allowing for
/// comprehensive tracking of the team's activities and outcomes.</remarks>
[DebuggerDisplay("Name: {Name,nq}")]
public sealed record Team(string Name, string Id)
{
	public string URL { get; set; } = "";
	public string Caption { get; set; } = "";
	public string ShortName { get; set; } = "";
	public int ClubId { get; set; }
	public string DivisionName { get; set; } = "";
	public string Captain { get; set; } = "";
	public string CaptainPhone { get; set; } = "";
	public string CaptainEmailAddress { get; set; } = "";
	public ImmutableList<Player>? Players { get; set; } = [];
	public ImmutableList<Fixture>? Fixtures { get; set; } = [];
	public ImmutableList<TeamResult>? Results { get; set; } = [];
	public int? LeaguePosition { get; set; }
	public int Played { get; set; }
	public int Won { get; set; }
	public int Drawn { get; set; }
	public int Lost { get; set; }
	public int SetsFor { get; set; }
	public int SetsAgainst { get; set; }
	public int Points { get; set; }
	public int PointsAdjustment { get; set; }
}

public static class TeamExtensions
{
	extension(Team)
	{
		public static Team Empty => new("", "")
		{
			URL = "",
			Caption = "",
			ShortName = "",
			ClubId = 0,
			DivisionName = "",
			Captain = "",
			CaptainPhone = "",
			CaptainEmailAddress = "",
			Players = [],
			Fixtures = [],
			Results = [],
			LeaguePosition = null,
			Played = 0,
			Won = 0,
			Drawn = 0,
			Lost = 0,
			SetsFor = 0,
			SetsAgainst = 0,
			Points = 0,
			PointsAdjustment = 0
		};
	}
}
