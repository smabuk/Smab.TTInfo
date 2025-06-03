namespace Smab.TTInfo.TT365.Models;

/// <summary>
/// Represents a unique identifier for a TT365 season.
/// </summary>
/// <remarks>This struct is a lightweight, immutable representation of a season identifier in the TT365 system. It
/// provides implicit and explicit conversion operators for seamless interaction with <see cref="string"/>
/// values.</remarks>
/// <param name="Value"></param>
public readonly record struct TT365SeasonId(string Value) : IComparable<TT365SeasonId>
{
	public override string ToString() => Value;
	public static implicit operator string(TT365SeasonId id) => id.Value;
	public static explicit operator TT365SeasonId(string value) => new(value);

	public int CompareTo(TT365SeasonId other)
		=> string.Compare(Value, other.Value, StringComparison.OrdinalIgnoreCase);
}
