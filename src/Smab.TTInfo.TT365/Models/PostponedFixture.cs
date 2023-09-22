namespace Smab.TTInfo.TT365.Models;

public record PostponedFixture : Fixture
{
	public string Reason { get; set; } = "";
}
