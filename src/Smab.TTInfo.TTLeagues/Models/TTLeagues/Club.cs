namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a club in TTLeagues, including contact and social details.
/// </summary>
/// <param name="Id">The unique identifier for the club.</param>
/// <param name="Name">The name of the club.</param>
/// <param name="ShortName">The short name of the club.</param>
/// <param name="Location">The location of the club.</param>
/// <param name="Linked">Indicates if the club is linked.</param>
/// <param name="Url">The website URL of the club.</param>
/// <param name="Secretaries">The secretaries object.</param>
/// <param name="Contact">The contact object.</param>
/// <param name="ContactId">The contact identifier.</param>
/// <param name="LinkId">The link identifier object.</param>
/// <param name="Affiliation">The affiliation object.</param>
/// <param name="Facebook">The Facebook handle.</param>
/// <param name="Twitter">The Twitter handle.</param>
/// <param name="Instagram">The Instagram handle.</param>
/// <param name="Email">The email address of the club.</param>
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
