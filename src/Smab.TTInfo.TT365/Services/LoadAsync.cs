using HtmlAgilityPack;

namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
	public async Task<T?> LoadAsync<T>(string ttinfoId, string? url, string fileName, string? cacheFolder = null, int? cacheHours = null)
	{
		string? contentString = null;
		T? returnValue = default;

		string folder = cacheFolder ?? CacheFolder;

		if (!Directory.Exists(folder)) {
			_ = Directory.CreateDirectory(folder);
		}

		fileName = fileName.ToLowerInvariant();
		string source = Path.Combine(folder, $"{CACHEFILE_PREFIX}{fileName}");
		bool refreshCache = File.GetLastWriteTimeUtc(source).AddHours(cacheHours ?? CacheHours) < timeProvider.GetUtcNow();

		if (!refreshCache || UseTestFiles || url is null) {
			if (File.Exists(source)) {
				contentString = LoadFileFromCache(fileName);
			}
		}

		if (string.IsNullOrWhiteSpace(contentString) && url is not null) {
			EnsureBaseAddress(ttinfoId);
			HttpResponseMessage? response = await httpClient.GetAsync(url);
			if (response.IsSuccessStatusCode) {
				contentString = await response.Content.ReadAsStringAsync();
				_ = SaveFileToCache(contentString, fileName);
			//} else if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable) {
			//	jsonString = LoadFile(fileName);
			} else {
				contentString = LoadFileFromCache(fileName);
			}
		}

		if (typeof(T).Name == "HtmlDocument") {
			HtmlDocument rv = new();
			rv.LoadHtml(contentString);
			//rv.Save(source);
			returnValue = (T?)Convert.ChangeType(rv, typeof(T));
		} else if (!string.IsNullOrWhiteSpace(contentString)) {
			returnValue = JsonSerializer.Deserialize<T>(contentString, JSON_SER_OPTIONS);
		}

		return returnValue;
	}

	private void EnsureBaseAddress(string ttinfoId)
	{
		httpClient.BaseAddress ??= new Uri($"{TT365_COM}/{ttinfoId}/");
	}
}
