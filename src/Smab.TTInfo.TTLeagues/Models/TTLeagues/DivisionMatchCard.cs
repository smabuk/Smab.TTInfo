namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;

/// <summary>
/// Represents a match card for a division within a competition, containing details about the competition, approval
/// status, layout, player labels, and printout layout.
/// </summary>
/// <param name="CompetitionId">The unique identifier of the competition associated with this match card.</param>
/// <param name="Approval">The approval status or details for the match card. The specific type and usage depend on the context of the
/// application.</param>
/// <param name="Layout">The layout configuration for the match card. This defines how the match card is visually structured.</param>
/// <param name="PlayerLabels">The labels or identifiers for players included in the match card. The specific type and usage depend on the context
/// of the application.</param>
/// <param name="PrintoutLayout">The identifier for the printout layout to be used when rendering the match card for printing.</param>
internal sealed record DivisionMatchCard(
	int     CompetitionId,
	object  Approval,
	object  Layout,
	object  PlayerLabels,
	int     PrintoutLayout
);

