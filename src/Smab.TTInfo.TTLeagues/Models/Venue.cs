﻿namespace Smab.TTInfo.TTLeagues.Models;
internal sealed record Venue(
	int? Id,
	string Name,
	string ShortName,
	string Contact,
	int? TypeId,
	string AddressLine1,
	string AddressLine2,
	string AddressLine3,
	string Town,
	string County,
	string Country,
	string Postcode,
	string Information,
	string Directions,
	string Parking,
	string Instructions,
	string Telephone,
	string Email,
	string Website,
	object Tables,
	string UserId,
	List<Club>? Clubs,
	bool? Linked,
	string Latitude,
	string Longitude,
	int? HeaderId,
	int? PreviewId,
	bool? Deleted
 );
