namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents a division within an organization, containing a collection of teams.
/// </summary>
/// <remarks>The <see cref="Division"/> record is immutable and provides a convenient way to group teams under a
/// specific division.</remarks>
/// <param name="Id">The unique identifier for the division.</param>
/// <param name="Name">The name of the division.</param>
/// <param name="Teams">The list of teams that belong to the division.</param>
public record Division(string Id, string Name, List<Team> Teams);
