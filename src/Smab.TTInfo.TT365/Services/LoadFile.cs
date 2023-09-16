namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
	public string? LoadFile(string fileName)
	{
		if (!Directory.Exists(CacheFolder)) {
			_ = Directory.CreateDirectory(CacheFolder);
		}

		string source = Path.Combine(CacheFolder, fileName);

		return File.Exists(source) switch
		{
			true  => File.ReadAllText(source),
			false => null,
		};
	}
}
