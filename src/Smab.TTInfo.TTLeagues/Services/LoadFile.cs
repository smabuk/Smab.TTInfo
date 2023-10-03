namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	public string? LoadFile(string fileName)
	{
		if (!Directory.Exists(CacheFolder)) {
			return null;
		}

		fileName = fileName.ToLowerInvariant();
		string source = Path.Combine(CacheFolder, $"{CACHEFILE_PREFIX}{fileName}");

		return File.Exists(source) switch
		{
			true => File.ReadAllText(source),
			false => null,
		};

	}
}
