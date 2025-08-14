namespace Smab.TTInfo.TT365.Models;

/// <summary>
/// Represents a unique identifier for a league in the TT365 system.
/// </summary>
/// <remarks>This type is a value object that encapsulates a league identifier as a string. It provides implicit
/// and explicit conversion operators for seamless interaction with string values.</remarks>
/// <param name="LeagueId"></param>
[JsonConverter(typeof(TT365LeagueIdConverter))]
public readonly record struct TT365LeagueId(string LeagueId)
{
	public override string ToString() => LeagueId;
	public static implicit operator string(TT365LeagueId id) => id.LeagueId;
	public static explicit operator TT365LeagueId(string value) => new(value);
}

/// <summary>
/// Converts a <see cref="TT365LeagueId"/> object to its string representation and vice versa.
/// </summary>
/// <remarks>This converter is designed to facilitate serialization and deserialization of <see
/// cref="TT365LeagueId"/>  objects by converting them to and from their string representations. It uses a creator
/// function to  instantiate <see cref="TT365LeagueId"/> objects and an extractor function to retrieve their string
/// values.</remarks>
public class TT365LeagueIdConverter : SingleValueConverter<TT365LeagueId, string>
{
	public TT365LeagueIdConverter() : base(creator => new TT365LeagueId(creator!), extractor => extractor.ToString()) { }
}
