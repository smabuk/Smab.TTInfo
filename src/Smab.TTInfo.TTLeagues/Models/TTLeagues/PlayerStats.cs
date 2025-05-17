namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents the statistical data and rankings of a player across various competitions and contexts.
/// </summary>
/// <remarks>This record encapsulates a player's performance metrics, rankings, and related data,  including
/// competition and team associations, results, averages, and form trends. It is designed to provide a comprehensive
/// view of a player's statistics for analysis or display purposes.</remarks>
/// <param name="Id"></param>
/// <param name="Competition"></param>
/// <param name="Division"></param>
/// <param name="Team"></param>
/// <param name="Club"></param>
/// <param name="Results"></param>
/// <param name="Average"></param>
/// <param name="Name"></param>
/// <param name="LocalRanking"></param>
/// <param name="NationalRanking"></param>
/// <param name="Form"></param>
/// <param name="Rankings"></param>
internal sealed record PlayerStats(
	string Id,
	IntKeyValue Competition,
	IntKeyValue Division,
	IntKeyValue Team,
	IntKeyValue Club,
	IReadOnlyList<PlayerResult> Results,
	PlayerAverage Average,
	string        Name,
	PlayerLocalRanking          LocalRanking,
	PlayerNationalRanking       NationalRanking,
	IReadOnlyList<PlayerForm>   Form,
	IReadOnlyList<object>       Rankings
);
