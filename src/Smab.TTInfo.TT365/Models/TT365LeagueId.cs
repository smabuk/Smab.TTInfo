namespace Smab.TTInfo.TT365.Models;

/// <summary>
/// Represents a unique identifier for a league in the TT365 system.
/// </summary>
/// <remarks>This type is a value object that encapsulates a league identifier as a string. It provides implicit
/// and explicit conversion operators for seamless interaction with string values.</remarks>
/// <param name="LeagueId"></param>
public readonly record struct TT365LeagueId(string LeagueId)
{
	public override string ToString() => LeagueId;
	public static implicit operator string(TT365LeagueId id) => id.LeagueId;
	public static explicit operator TT365LeagueId(string value) => new(value);
}
