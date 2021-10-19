namespace Smab.TTInfo.Models.TT365;

public class FixturesView
{
	public string Caption { get; set; } = "";
	public string URL { get; set; } = "";
	public FixturesViewOptions Options { get; set; } = new();
	public ICollection<Fixture>? Fixtures { get; set; }
}
