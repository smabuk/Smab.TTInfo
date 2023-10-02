namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record WebsitesHostPrimary(
	int     Id,
	object  ParentId,
	object  Parent,
	string  Title,
	string  SubTitle,
	string  Body,
	string  Name,
	int     Ordering,
	int     Preset,
	object  Content,
	string  Url,
	IReadOnlyList<object> Items,
	int     Type,
	bool    Enabled
);
