namespace Smab.TTInfo.TTLeagues.Models;

/// <summary>
/// Represents a lookup value with a specific type, identifier, and name.
/// </summary>
/// <remarks>This record is used to encapsulate a value that can be identified by its type, ID, and name. It is
/// commonly used in scenarios where a key-value pair or reference data is required.</remarks>
/// <param name="Type"></param>
/// <param name="Id"></param>
/// <param name="Name"></param>
public record LookupValue(LookupType Type, int Id, string Name);
