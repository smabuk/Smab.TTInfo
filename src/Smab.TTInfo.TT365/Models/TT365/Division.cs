namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents a division within an organization, containing a collection of teams and associated metadata.
/// </summary>
/// <remarks>A division is identified by a unique ID and has a name. It also maintains a list of teams and
/// provides a count of the total number of teams within the division.</remarks>
/// <param name="Id"></param>
/// <param name="Name"></param>
public record Division(string Id, string Name)
{
	public List<Team> Teams { get; set; } = [];
	public int TeamCount => Teams?.Count ?? 0;
}
