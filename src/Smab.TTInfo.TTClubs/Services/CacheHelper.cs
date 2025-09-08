namespace Smab.TTInfo.TTClubs.Services;

/// <summary>
/// Provides static methods for reading from and writing to a JSON file cache.
/// </summary>
public static class CacheHelper
{
	private static readonly Lock _fileAppendLock = new();
	private static readonly Lock _tournamentFileLock = new();

	/// <summary>
	/// Loads the contents of a cached file as a string.
	/// </summary>
	/// <param name="fileName">The cache file name.</param>
	/// <param name="cacheFolder">The cache folder path.</param>
	/// <returns>The file contents as a string, or null if not found.</returns>
	public static string? LoadFileFromCache(string fileName, string cacheFolder)
	{
		if (!Directory.Exists(cacheFolder)) {
			return null;
		}

		fileName = fileName.ToLowerInvariant();
		string source = Path.Combine(cacheFolder, fileName);

		return File.Exists(source) ? File.ReadAllText(source) : null;
	}

	/// <summary>
	/// Saves the specified contents to a cache file.
	/// </summary>
	/// <param name="contents">The contents to save.</param>
	/// <param name="fileName">The cache file name.</param>
	/// <param name="cacheFolder">The cache folder path.</param>
	/// <returns>True if the file was saved successfully.</returns>
	public static bool SaveFileToCache(string contents, string fileName, string cacheFolder)
	{
		lock (_tournamentFileLock) {
			if (!Directory.Exists(cacheFolder)) {
				_ = Directory.CreateDirectory(cacheFolder);
			}

			fileName = fileName.ToLowerInvariant();
			string destination = Path.Combine(cacheFolder, fileName);

			File.WriteAllText(destination, contents);

			return true;
		}
	}

	public static bool AppendTextToFileInCache(string contents, string fileName, string cacheFolder)
	{
		lock (_fileAppendLock) {
			if (!Directory.Exists(cacheFolder)) {
				_ = Directory.CreateDirectory(cacheFolder);
			}

			fileName = fileName.ToLowerInvariant();
			string destination = Path.Combine(cacheFolder, fileName);
			File.AppendAllText(destination, contents);
			return true;
		}
	}
}
