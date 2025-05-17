namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// 
/// </summary>
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
internal sealed record TenantsHostOrganisation(
	int     Id,
	string  Name,
	string  Logo,
	int     Import,
	string  Logins,
	IReadOnlyList<int> LoginModes,
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
