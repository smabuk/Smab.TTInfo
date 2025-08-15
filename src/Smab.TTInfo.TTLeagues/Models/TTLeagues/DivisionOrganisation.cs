namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents the organizational details of a division, including its identity, branding,  configuration settings, and
/// social media links.
/// </summary>
/// <remarks>This record encapsulates various properties related to a division's organization, such as  its name,
/// logo, website, and social media presence, as well as configuration flags for  registration, reporting, and
/// membership requirements.</remarks>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="Logo"></param>
/// <param name="Import"></param>
/// <param name="Logins"></param>
/// <param name="LoginModes"></param>
/// <param name="RegistrationEnabled"></param>
/// <param name="ReportingEnabled"></param>
/// <param name="CompeteMembershipRequired"></param>
/// <param name="Website"></param>
/// <param name="News"></param>
/// <param name="Privacy"></param>
/// <param name="Terms"></param>
/// <param name="Twitter"></param>
/// <param name="Facebook"></param>
/// <param name="Instagram"></param>
public sealed record DivisionOrganisation(
	int     Id,
	string  Name,
	string  Logo,
	int     Import,
	string  Logins,
	ImmutableList<int> LoginModes,
	bool    RegistrationEnabled,
	bool    ReportingEnabled,
	bool    CompeteMembershipRequired,
	string  Website,
	string  News,
	string  Privacy,
	string  Terms,
	string  Twitter,
	string  Facebook,
	string  Instagram
);

