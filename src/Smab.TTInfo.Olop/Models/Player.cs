namespace Smab.TTInfo.Olop.Models;

[JsonDerivedType(typeof(NamedPlayer), typeDiscriminator: "NamedPlayer")]
[JsonDerivedType(typeof(NoPlayer), typeDiscriminator: "NoPlayer")]
[JsonDerivedType(typeof(Bye), typeDiscriminator: "Bye")]

public abstract record Player(string Name);

public record NamedPlayer(string Name) : Player(Name);

public record NoPlayer() : Player("");
public record Bye(int? ByeNo = null) : Player(ByeNo is null ? "BYE" :$"BYE {ByeNo}");

public static class PlayerExtensions
{
	extension(string name)
	{
		public Player ToPlayer() => name switch
		{
			null => new NoPlayer(),
			"" => new NoPlayer(),
			"BYE" => new Bye(),
			_ when name.StartsWith("BYE ") => new Bye(int.Parse(name[4..])),
			_ => new NamedPlayer(name)
		};
	}

	//public static string GetDisplayName(this Player player) => player switch
	//{
	//	NamedPlayer namedPlayer => namedPlayer.PlayerName,
	//	Bye bye => bye.ByeNo is null ? "BYE" : $"BYE {bye.ByeNo}",
	//	NoPlayer => "",
	//	_ => throw new ArgumentOutOfRangeException(nameof(player), player, null)
	//};
}
