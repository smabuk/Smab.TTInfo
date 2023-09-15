namespace Smab.TTInfo;
public partial class TTLeaguesReader
{
	public required string CacheFolder;
	public int CacheHours = 12;
	public bool UseTestFiles { get; set; } = false;

	private readonly static System.Globalization.CultureInfo gbCulture = new("en-GB");
	//private static readonly string tt365com = $"{"https"}://www.tabletennis365.com";

}
