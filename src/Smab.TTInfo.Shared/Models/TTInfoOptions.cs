namespace Smab.TTInfo.Shared.Models;

/// <summary>
/// Base options for TTInfo configuration.
/// </summary>
public abstract class TTInfoOptions
{
	/// <summary>
	/// The folder path for caching data.
	/// </summary>
	public string CacheFolder  { get; set; } = @"cache";

	/// <summary>
	/// The number of hours to keep cached data.
	/// </summary>
	public int    CacheHours   { get; set; } = 6;

	/// <summary>
	/// Indicates whether to use test files instead of live data.
	/// </summary>
	public bool   UseTestFiles { get; set; } = false;
}
