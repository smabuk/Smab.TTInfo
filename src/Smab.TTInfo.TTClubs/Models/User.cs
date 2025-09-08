namespace Smab.TTInfo.TTClubs.Models;

public sealed record User
(
	string Id,
	string FirstName,
	string LastName,
	string UserName,
	string Email,
	string DisplayName,
	int LinkId,
	string Initials,
	object ImageId,
	[Display(Name = "TTE #")]
	int MembershipNo,
	string MembershipType,
	int Gender,
	DateTime Dob,
	int Age,
	DateTime Expiry,
	object Roles,
	object Notifications,
	object Type,
	object Clubs,
	string AddressLine1,
	string AddressLine2,
	string City,
	string County,
	string Country,
	string Postcode,
	string PhoneNumber,
	string Uuid
);
