namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	public string? LoadFile(string fileName, int? cacheHours = null)
	{
		if (!Directory.Exists(CacheFolder)) {
			_ = Directory.CreateDirectory(CacheFolder);
		}

		string source = Path.Combine(CacheFolder, $"{CACHEFILE_PREFIX}{fileName}");

		return File.Exists(source) switch
		{
			true => File.ReadAllText(source),
			false => null,
		};

	}
}
