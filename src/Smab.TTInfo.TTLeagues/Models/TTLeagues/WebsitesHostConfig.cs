namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents the configuration settings for hosting websites, including email settings, UI preferences, competition
/// settings, and other related options.
/// </summary>
/// <remarks>This record encapsulates various configuration options for a website host, such as email addresses,
/// UI elements visibility, competition-related preferences, and redirection behavior. It is designed to provide a
/// centralized way to manage these settings.</remarks>
/// <param name="ReplyEmail"></param>
/// <param name="FromEmail"></param>
/// <param name="HomeHeader"></param>
/// <param name="SubHeader"></param>
/// <param name="ResultsCarousel"></param>
/// <param name="AppEnabled"></param>
/// <param name="SubmissionType"></param>
/// <param name="ApprovalType"></param>
/// <param name="CompetitionMenuDisplay"></param>
/// <param name="Visibility"></param>
/// <param name="RedirectUrl"></param>
/// <param name="AutoScrollCheck"></param>
/// <param name="CompetitionPreference"></param>
/// <param name="MaxArticles"></param>
/// <param name="Published"></param>
public sealed record WebsitesHostConfig(
	string  ReplyEmail,
	string  FromEmail,
	bool    HomeHeader,
	bool    SubHeader,
	bool    ResultsCarousel,
	bool    AppEnabled,
	int     SubmissionType,
	int     ApprovalType,
	int     CompetitionMenuDisplay,
	int     Visibility,
	string  RedirectUrl,
	bool    AutoScrollCheck,
	int     CompetitionPreference,
	int     MaxArticles,
	object  Published
);
