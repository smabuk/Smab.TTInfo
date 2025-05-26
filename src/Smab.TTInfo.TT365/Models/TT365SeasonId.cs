namespace Smab.TTInfo.TT365.Models;

/// <summary>
/// Represents a unique identifier for a TT365 season.
/// </summary>
/// <remarks>This struct is a lightweight, immutable representation of a season identifier in the TT365 system. It
/// provides implicit and explicit conversion operators for seamless interaction with <see cref="string"/>
/// values.</remarks>
/// <param name="SeasonId"></param>
public readonly record struct TT365SeasonId(string SeasonId)
{
	public override string ToString() => SeasonId;
	public static implicit operator string(TT365SeasonId id) => id.SeasonId;
	public static explicit operator TT365SeasonId(string value) => new(value);
}
