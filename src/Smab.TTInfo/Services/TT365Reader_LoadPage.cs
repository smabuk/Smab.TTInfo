using HtmlAgilityPack;

namespace Smab.TTInfo;

public partial class TT365Reader
{
	public async Task<HtmlDocument> LoadPage(string url, string fileName)
	{
		string html = "";
		HtmlDocument doc = new();

        if (!Directory.Exists(CacheFolder))
        {
			Directory.CreateDirectory(CacheFolder);
        }

		string source = Path.Combine(CacheFolder, fileName);
		bool refreshCache = File.GetLastWriteTimeUtc(source).AddHours(CacheHours) < DateTime.UtcNow;

		bool docLoadSuccessful = false;
		if (!refreshCache || UseTestFiles)
		{
			if (File.Exists(source))
			{
				doc.Load(source);
				docLoadSuccessful = true;
			}
		}
		if (!docLoadSuccessful)
		{
			using (HttpClient client = new())
			{
				html = await client.GetStringAsync(url);
			}
			doc.LoadHtml(html);
			doc.Save(source);
		}
		return doc;
	}
}

