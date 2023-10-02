namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record Club(
	int?   Id,
	string Name,
	string ShortName,
	string Location,
	bool?  Linked,
	string Url,
	object Secretaries,
	object Contact,
	string ContactId,
	object LinkId,
	object Affiliation,
	string Facebook,
	string Twitter,
	string Instagram,
	string Email
 );
