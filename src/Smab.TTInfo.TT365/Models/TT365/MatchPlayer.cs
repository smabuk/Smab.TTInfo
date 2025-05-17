namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents a player in a match, including their name, identifier, sets won, and Player of the Match status.
/// </summary>
/// <param name="Name">The name of the player.</param>
/// <param name="Id">The unique identifier for the player.</param>
/// <param name="SetsWon">The number of sets won by the player during the match.</param>
/// <param name="PoM">A value indicating whether the player was awarded Player of the Match. <see langword="true"/> if the player was the
/// Player of the Match; otherwise, <see langword="false"/>.</param>
public record MatchPlayer(string Name, int Id, int SetsWon, bool PoM);
