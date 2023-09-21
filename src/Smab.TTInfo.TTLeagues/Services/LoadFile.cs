﻿namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
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