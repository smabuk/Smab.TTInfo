namespace Smab.TTInfo.Olop.Models;

[JsonDerivedType(typeof(MainTournament), typeDiscriminator: "Main")]
[JsonDerivedType(typeof(ConsolationTournament), typeDiscriminator: "Consolation")]
public abstract record Tournament(List<Round> Rounds);
public record MainTournament(List<Round> Rounds) : Tournament(Rounds);
public record ConsolationTournament(List<Round> Rounds) : Tournament(Rounds);
