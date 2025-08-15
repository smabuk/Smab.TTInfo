namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a key-value pair where the key is an optional integer and the value is a string.
/// </summary>
/// <remarks>This type is commonly used to store or transfer data where a nullable integer key is associated with
/// a string value. The <see cref="Key"/> property can be null, indicating the absence of a key.</remarks>
/// <param name="Key"></param>
/// <param name="Value"></param>
public sealed record IntKeyValue(int Key, string Value);
