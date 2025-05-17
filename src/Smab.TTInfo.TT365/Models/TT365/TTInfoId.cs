namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents a strongly-typed TTInfo identifier.
/// </summary>
public readonly record struct TT365LeagueId(string LeagueId)
{
	public override string ToString() => LeagueId;
	public static implicit operator string(TT365LeagueId id) => id.LeagueId;
	public static explicit operator TT365LeagueId(string value) => new(value);
}
