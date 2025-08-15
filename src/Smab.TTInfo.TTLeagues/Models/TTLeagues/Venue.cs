namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a venue with detailed information such as address, contact details, and associated clubs.
/// </summary>
/// <remarks>This record encapsulates all relevant data for a venue, including its location, contact information, 
/// and additional metadata such as parking instructions, directions, and linked clubs.  It is designed to be immutable
/// and thread-safe.</remarks>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="ShortName"></param>
/// <param name="Contact"></param>
/// <param name="TypeId"></param>
/// <param name="AddressLine1"></param>
/// <param name="AddressLine2"></param>
/// <param name="AddressLine3"></param>
/// <param name="Town"></param>
/// <param name="County"></param>
/// <param name="Country"></param>
/// <param name="Postcode"></param>
/// <param name="Information"></param>
/// <param name="Directions"></param>
/// <param name="Parking"></param>
/// <param name="Instructions"></param>
/// <param name="Telephone"></param>
/// <param name="Email"></param>
/// <param name="Website"></param>
/// <param name="Tables"></param>
/// <param name="UserId"></param>
/// <param name="Clubs"></param>
/// <param name="Linked"></param>
/// <param name="Latitude"></param>
/// <param name="Longitude"></param>
/// <param name="HeaderId"></param>
/// <param name="PreviewId"></param>
/// <param name="Deleted"></param>
public sealed record Venue(
	int?    Id,
	string  Name,
	string  ShortName,
	string  Contact,
	int?    TypeId,
	string  AddressLine1,
	string  AddressLine2,
	string  AddressLine3,
	string  Town,
	string  County,
	string  Country,
	string  Postcode,
	string  Information,
	string  Directions,
	string  Parking,
	string  Instructions,
	string  Telephone,
	string  Email,
	string  Website,
	object  Tables,
	string  UserId,
	ImmutableList<Club>? Clubs,
	bool?   Linked,
	string  Latitude,
	string  Longitude,
	int?    HeaderId,
	int?    PreviewId,
	bool?   Deleted
 );
