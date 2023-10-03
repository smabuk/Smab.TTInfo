namespace Smab.TTInfo.TTLeagues.Models.TTLeagues;
internal sealed record PlayerGame(
	int? For,
	int? Against
)
{
	public override string ToString() => $"{For}-{Against}";
};
