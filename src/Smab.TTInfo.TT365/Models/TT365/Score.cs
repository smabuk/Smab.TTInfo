namespace Smab.TTInfo.TT365.Models.TT365;

/// <summary>
/// Represents a score with two components, typically used to track results in a game or competition.
/// </summary>
/// <remarks>The <see cref="Score"/> record provides a simple way to encapsulate two related score values.  It is
/// immutable and supports value-based equality.</remarks>
/// <param name="Score1"></param>
/// <param name="Score2"></param>
public record class Score(int Score1, int Score2)
{
	/// <summary>
	/// Returns a string representation of the current object, formatted as a score pair.
	/// </summary>
	/// <returns>A string in the format "<c>Score1-Score2</c>", where <c>Score1</c> and <c>Score2</c> represent the respective
	/// scores.</returns>
	public override string ToString() => $"{Score1}-{Score2}";
}
