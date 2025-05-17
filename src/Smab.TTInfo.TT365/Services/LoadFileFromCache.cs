namespace Smab.TTInfo.TT365.Services;

/// <summary>
/// Provides methods to load files from the local cache for TT365.
/// </summary>
public sealed partial class TT365Reader
{
	/// <summary>
	/// Loads the contents of a cached file as a string.
	/// </summary>
	/// <param name="fileName">The cache file name.</param>
	/// <param name="cacheFolder">The cache folder path (optional).</param>
	/// <returns>The file contents as a string, or null if not found.</returns>
	public string? LoadFileFromCache(string fileName, string? cacheFolder = null)
	{
		string folder = cacheFolder ?? CacheFolder;

		if (!Directory.Exists(folder)) {
			return null;
		}

		fileName = fileName.ToLowerInvariant();
		string source = Path.Combine(folder, $"{CACHEFILE_PREFIX}{fileName}");

		return File.Exists(source) switch
		{
			true => File.ReadAllText(source),
			false => null,
		};
	}
}
