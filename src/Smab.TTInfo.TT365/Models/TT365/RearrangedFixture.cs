namespace Smab.TTInfo.TT365.Models.TT365;

public record RearrangedFixture : Fixture
{
	public DateOnly OriginalDate {  get; set; }
	public string Reason { get; set; } = "";
}
