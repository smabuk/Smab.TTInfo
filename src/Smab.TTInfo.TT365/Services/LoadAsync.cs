using HtmlAgilityPack;

namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
	public async Task<T?> LoadAsync<T>(string ttinfoId, string? url, string fileName, string? cacheFolder = null, int? cacheHours = null)
	{
		string? jsonString = null;
		T? returnValue = default;

		string folder = cacheFolder ?? CacheFolder;

		if (!Directory.Exists(folder)) {
			_ = Directory.CreateDirectory(folder);
		}

		fileName = fileName.ToLowerInvariant();
		string source = Path.Combine(folder, $"{CACHEFILE_PREFIX}{fileName}");
		bool refreshCache = File.GetLastWriteTimeUtc(source).AddHours(cacheHours ?? CacheHours) < DateTime.UtcNow;

		if (!refreshCache || UseTestFiles || url is null) {
			if (File.Exists(source)) {
				jsonString = LoadFileFromCache(fileName);
			}
		}

		if (string.IsNullOrWhiteSpace(jsonString) && url is not null) {
			using HttpClient client = CreateHttpClient(ttinfoId);
			HttpResponseMessage? response = await client.GetAsync(url);
			if (response.IsSuccessStatusCode) {
				jsonString = await response.Content.ReadAsStringAsync();
				_ = SaveFileToCache(jsonString, fileName);
			//} else if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable) {
			//	jsonString = LoadFile(fileName);
			} else {
				jsonString = LoadFileFromCache(fileName);
			}
		}

		if (typeof(T).Name == "HtmlDocument") {
			HtmlDocument rv = new();
			rv.LoadHtml(jsonString);
			rv.Save(source);
			returnValue = (T?)Convert.ChangeType(rv, typeof(T));
		} else if (!string.IsNullOrWhiteSpace(jsonString)) {
			returnValue = JsonSerializer.Deserialize<T>(jsonString, JSON_SER_OPTIONS);
		}

		return returnValue;
	}
}
