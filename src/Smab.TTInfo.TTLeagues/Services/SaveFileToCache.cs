using Smab.TTInfo.Shared.Helpers;

namespace Smab.TTInfo.TTLeagues.Services;

/// <summary>
/// Provides functionality to read and cache TTLeagues data.
/// </summary>
public sealed partial class TTLeaguesReader
{
	/// <summary>
	/// Saves a file to the cache for a given file name.
	/// </summary>
	/// <param name="contents">The content to save.</param>
	/// <param name="fileName">The name of the file to save.</param>
	/// <param name="cacheFolder">The optional cache folder path.</param>
	/// <returns>True if the file was successfully saved.</returns>
	public bool SaveFileToCache(string contents, string fileName, string? cacheFolder = null)
		=> CacheHelper.SaveFileToCache(contents, $"{CACHEFILE_PREFIX}{fileName}", cacheFolder ?? CacheFolder);
}
