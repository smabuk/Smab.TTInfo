using Smab.TTInfo.Shared.Helpers;

namespace Smab.TTInfo.TT365.Services;

/// <summary>
/// Provides methods to save files to the local cache for TT365.
/// </summary>
public sealed partial class TT365Reader
{
	/// <summary>
	/// Saves the specified contents to a cache file.
	/// </summary>
	/// <param name="contents">The contents to save.</param>
	/// <param name="fileName">The cache file name.</param>
	/// <param name="cacheFolder">The cache folder path (optional).</param>
	/// <returns>True if the file was saved successfully.</returns>
	public bool SaveFileToCache(string contents, string fileName, string? cacheFolder = null)
		=> CacheHelper.SaveFileToCache(contents, $"{CACHEFILE_PREFIX}{fileName}", cacheFolder ?? CacheFolder);
}
