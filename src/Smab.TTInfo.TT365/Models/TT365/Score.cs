namespace Smab.TTInfo.TT365.Models.TT365;
public record class Score(int Score1, int Score2)
{
	public override string ToString() => $"{Score1}-{Score2}";
}
