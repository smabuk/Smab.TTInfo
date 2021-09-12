using HtmlAgilityPack;

namespace Smab.TTInfo;

public partial class TT365Reader
{
	public async Task<HtmlDocument> LoadPage(string url, string fileName)
	{
		string html = "";
		string source = "";

		HtmlDocument doc = new();
		bool docLoadSuccessful = false;
		if (UseTestFiles)
		{
			source = Path.Combine(CacheFolder, fileName);
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
			if (UseTestFiles)
			{
				doc.Save(source);
			}
		}
		return doc;
	}
}

