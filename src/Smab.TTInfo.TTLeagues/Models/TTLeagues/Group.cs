namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a group of matches with associated metadata, such as type, date, range, week, and title.
/// </summary>
/// <remarks>This record is used to encapsulate a collection of matches and their related properties. The <see
/// cref="Matches"/> property contains the list of matches, while other properties provide additional contextual
/// information about the group, such as its type, date, and title.</remarks>
/// <param name="Matches"></param>
/// <param name="Type"></param>
/// <param name="Date"></param>
/// <param name="Range"></param>
/// <param name="Week"></param>
/// <param name="Title"></param>
public sealed record Group
(
	List<Match>     Matches,
	int             Type,
	DateTimeOffset? Date,
	int             Range,
	int?            Week,
	string          Title
);
