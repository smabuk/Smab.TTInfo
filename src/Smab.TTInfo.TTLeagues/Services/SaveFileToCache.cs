namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	public bool SaveFileToCache(string contents, string fileName, string? cacheFolder = null)
	{
		string folder = cacheFolder ?? CacheFolder;

		if (!Directory.Exists(folder))
		{
			_ = Directory.CreateDirectory(folder);
		}

		fileName = fileName.ToLowerInvariant();
		string destination = Path.Combine(folder,  $"{CACHEFILE_PREFIX}{fileName}");

		File.WriteAllText(destination, contents);

		return true;
	}
}

