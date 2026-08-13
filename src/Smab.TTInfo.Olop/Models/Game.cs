namespace Smab.TTInfo.Olop.Models;

public record Game(int Points1, int Points2)
{
	public override string ToString() => $"{Points1}-{Points2}";
};
