namespace Smab.TTInfo.TT365.Models;

/// <summary>
/// Represents a unique identifier for a TT365 season.
/// </summary>
/// <remarks>This struct is a lightweight, immutable representation of a season identifier in the TT365 system. It
/// provides implicit and explicit conversion operators for seamless interaction with <see cref="string"/>
/// values.</remarks>
/// <param name="Value"></param>
[JsonConverter(typeof(TT365SeasonIdConverter))]
public readonly record struct TT365SeasonId(string Value) : IComparable<TT365SeasonId>
{
	public override string ToString() => Value;
	public static implicit operator string(TT365SeasonId id) => id.Value;
	public static explicit operator TT365SeasonId(string value) => new(value);

	public int CompareTo(TT365SeasonId other)
		=> string.Compare(Value, other.Value, StringComparison.OrdinalIgnoreCase);
	public string ToDisplay() => Value.Replace('_', ' ');
}


public class TT365SeasonIdConverter : SingleValueConverter<TT365SeasonId, string>
{
	public TT365SeasonIdConverter() : base(creator => new TT365SeasonId(creator!), extractor => extractor.ToString()) { }
}
