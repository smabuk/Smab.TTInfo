namespace Smab.TTInfo;

public sealed partial class TT365Reader : ITT365Reader
{
	public required string CacheFolder;
	public int CacheHours = 12;
	public bool UseTestFiles { get; set; } = false;

	private readonly System.Globalization.CultureInfo gbCulture = new("en-GB");

	public TT365Reader()
	{
	}
}
