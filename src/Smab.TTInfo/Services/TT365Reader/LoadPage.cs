using HtmlAgilityPack;

namespace Smab.TTInfo;

public partial class TT365Reader
{
	public async Task<HtmlDocument> LoadPage(string url, string fileName, int? cacheHours = null)
	{
		string html;
		HtmlDocument doc = new();

		if (!Directory.Exists(CacheFolder))
		{
			Directory.CreateDirectory(CacheFolder);
		}

		string source = Path.Combine(CacheFolder, fileName);
		bool refreshCache = File.GetLastWriteTimeUtc(source).AddHours(cacheHours ?? CacheHours) < DateTime.UtcNow;

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
			using HttpClient client = new();
			HttpResponseMessage? response = await client.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				html = await response.Content.ReadAsStringAsync();
				doc.LoadHtml(html);
				doc.Save(source);
			}
			else if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
			{
				if (File.Exists(source))
				{
					doc.Load(source);
					docLoadSuccessful = true;
				}
			}
			else
			{
				if (File.Exists(source))
				{
					doc.Load(source);
					docLoadSuccessful = true;
				}
			}
		}
		return doc;
	}
}

