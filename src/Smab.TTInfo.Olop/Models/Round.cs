namespace Smab.TTInfo.Olop.Models;

[JsonDerivedType(typeof(MainRound), typeDiscriminator: "Main")]
[JsonDerivedType(typeof(ConsolationRound), typeDiscriminator: "Consolation")]
public abstract record Round(RoundType Type, string Name, List<Match> Matches);
public record MainRound(RoundType Type, string Name, List<Match> Matches) : Round(Type, Name, Matches);
public record ConsolationRound(RoundType Type, string Name, List<Match> Matches) : Round(Type, Name, Matches);
