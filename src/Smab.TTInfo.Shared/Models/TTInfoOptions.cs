namespace Smab.TTInfo.Shared.Models;

public abstract class TTInfoOptions
{
	public string CacheFolder  { get; set; } = @"cache";
	public int    CacheHours   { get; set; } = 6;
	public bool   UseTestFiles { get; set; } = false;
}
