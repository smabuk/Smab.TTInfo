namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record WebsitesHostConfig(
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
