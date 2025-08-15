namespace Smab.TTInfo.TTLeagues.Models;

/// <summary>
/// Represents the type of entity to be looked up in a system.
/// </summary>
/// <remarks>This enumeration is used to specify the category of an entity, such as a club, competition, or venue,
/// when performing lookups or filtering operations. Each value corresponds to a distinct entity type.</remarks>
public enum LookupType
{
	Club,
	Competition,
	Division,
	Team,
	Venue,
}
