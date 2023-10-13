namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
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

