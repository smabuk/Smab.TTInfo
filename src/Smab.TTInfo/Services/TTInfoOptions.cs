namespace Smab.TTInfo.Services;

public class TTInfoOptions
{
	public string CacheFolder { get; set; } = @"cache";
	public int CacheHours { get; set; } = 12;
	public bool UseTestFiles { get; set; } = false;
}
