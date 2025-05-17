namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a key-value pair where the key is a nullable string and the value is a non-nullable string.
/// </summary>
/// <remarks>This type is commonly used to store or transfer key-value pairs where the key may be null,  but the
/// value is always required. It is immutable and provides value-based equality semantics.</remarks>
/// <param name="Key"></param>
/// <param name="Value"></param>
internal record StringKeyValue(string? Key, string Value);
