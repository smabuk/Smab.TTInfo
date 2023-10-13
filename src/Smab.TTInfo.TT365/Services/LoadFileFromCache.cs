namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
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
