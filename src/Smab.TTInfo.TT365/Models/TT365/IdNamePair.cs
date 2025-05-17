namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents a pair of an identifier and a name.
/// </summary>
/// <remarks>This record is commonly used to associate a unique identifier with a descriptive name.</remarks>
/// <param name="Id">The unique identifier for the pair. This value cannot be null.</param>
/// <param name="Name">The name associated with the identifier. This value cannot be null.</param>
public record IdNamePair(string Id, string Name);
