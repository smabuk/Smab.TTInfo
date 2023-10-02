namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
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

