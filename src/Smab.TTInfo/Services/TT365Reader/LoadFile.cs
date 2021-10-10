namespace Smab.TTInfo;

public partial class TT365Reader
{
	public string? LoadFile(string fileName)
	{
		if (!Directory.Exists(CacheFolder))
		{
			Directory.CreateDirectory(CacheFolder);
		}

		string source = Path.Combine(CacheFolder, fileName);

		if (File.Exists(source))
		{
			return File.ReadAllText(source);
		}

		return null;
	}
}

