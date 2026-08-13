namespace Smab.TTInfo.Olop.Models;

[JsonDerivedType(typeof(MainRound), typeDiscriminator: "MainRound")]
[JsonDerivedType(typeof(ConsolationRound), typeDiscriminator: "ConsolationRound")]
public abstract record Round(string Name, List<Match> Matches);
public record MainRound(string Name, List<Match> Matches) : Round(Name, Matches);
public record ConsolationRound(string Name, List<Match> Matches) : Round(Name, Matches);
