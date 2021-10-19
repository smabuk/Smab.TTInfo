using System.Diagnostics;

namespace Smab.TTInfo.Models.TT365;

[DebuggerDisplay("CompletedFixture: {Date,nq} - {HomeTeam,nq} ({ForHome,nq}) vs ({ForAway,nq}) {AwayTeam,nq}")]
public record CompletedFixture : Fixture
{
}
