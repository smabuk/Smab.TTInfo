namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents the reserve settings for a division in a competition.
/// </summary>
/// <remarks>This record encapsulates various configuration options related to team and player reserves, including
/// step-up limits, transfer permissions, and override settings.</remarks>
/// <param name="CompetitionId"></param>
/// <param name="Type"></param>
/// <param name="MaxStepUp"></param>
/// <param name="MaxTeamStepUp"></param>
/// <param name="StepUpType"></param>
/// <param name="TransferPlayer"></param>
/// <param name="MultipleTeams"></param>
/// <param name="OverrideEnabled"></param>
internal sealed record DivisionReserves(
	int     CompetitionId,
	int     Type,
	int     MaxStepUp,
	object  MaxTeamStepUp,
	int     StepUpType,
	bool    TransferPlayer,
	bool    MultipleTeams,
	bool    OverrideEnabled
);

