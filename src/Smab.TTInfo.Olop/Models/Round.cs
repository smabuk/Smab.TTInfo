namespace Smab.TTInfo.Olop.Models;

[JsonDerivedType(typeof(MainRound), typeDiscriminator: "MainRound")]
[JsonDerivedType(typeof(ConsolationRound), typeDiscriminator: "ConsolationRound")]
public abstract record Round(int RoundNo, string Name, List<Match> Matches);
public record MainRound(int RoundNo, string Name, List<Match> Matches) : Round(RoundNo, Name, Matches);
public record ConsolationRound(int RoundNo, string Name, List<Match> Matches) : Round(RoundNo, Name, Matches);
