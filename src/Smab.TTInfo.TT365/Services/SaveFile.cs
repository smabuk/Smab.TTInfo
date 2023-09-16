namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
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

