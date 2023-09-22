namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	public bool SaveFile(string contents, string fileName)
	{
		if (!Directory.Exists(CacheFolder))
		{
			_ = Directory.CreateDirectory(CacheFolder);
		}

		string destination = Path.Combine(CacheFolder, fileName);

		File.WriteAllText(destination, contents);

		return true;
	}
}

