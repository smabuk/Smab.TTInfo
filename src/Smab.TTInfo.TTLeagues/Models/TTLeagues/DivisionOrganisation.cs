namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record DivisionOrganisation(
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

