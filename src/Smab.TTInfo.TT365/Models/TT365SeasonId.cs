namespace Smab.TTInfo.TT365.Models;

/// <summary>
/// Represents a unique identifier for a season in the TT365 system, encapsulating its string representation and
/// providing comparison and equality operations. The identifier is normalized to use underscores instead of spaces for
/// consistency.
/// </summary>
[JsonConverter(typeof(TT365SeasonIdConverter))]
public readonly record struct TT365SeasonId : IComparable<TT365SeasonId>, IEquatable<TT365SeasonId>
{
	public TT365SeasonId(string value)
	{
		Value = NormalizeWithUnderscore(value);
	}

	public string Value { get; }

	public override string ToString() => Value;

	public static implicit operator string(TT365SeasonId id) => id.Value;

	public static explicit operator TT365SeasonId(string value) => new(NormalizeWithUnderscore(value));

	public int CompareTo(TT365SeasonId other) => string.Compare(Value, other.Value, StringComparison.OrdinalIgnoreCase);

	public bool Equals(TT365SeasonId other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

	public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

	public string ToDisplay() => Value.Replace('_', ' ');

	private static string NormalizeWithUnderscore(string? value) => value?.Replace(' ', '_') ?? throw new ArgumentNullException(nameof(value), "Season ID value cannot be null.");
}

/// <summary>
/// Converts a <see cref="TT365SeasonId"/> to its string representation and vice versa.
/// </summary>
/// <remarks>This converter is designed to facilitate serialization and deserialization of <see
/// cref="TT365SeasonId"/>  objects by converting them to and from their string representations. It uses a creator
/// function to  instantiate <see cref="TT365SeasonId"/> from a string and an extractor function to obtain the string
/// representation of a <see cref="TT365SeasonId"/>.</remarks>
public class TT365SeasonIdConverter : SingleValueConverter<TT365SeasonId, string>
{
	public TT365SeasonIdConverter() : base(creator => new TT365SeasonId(creator!), extractor => extractor.ToString()) { }
}
