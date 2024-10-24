namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	public async Task<T?> LoadJsonAsync<T>(string ttinfoId, string? url, string fileName, string? cacheFolder = null, int? cacheHours = null)
	{
		string? jsonString = null;
		T? returnValue = default;

		string folder = cacheFolder ?? CacheFolder;

		if (!Directory.Exists(folder)) {
			_ = Directory.CreateDirectory(folder);
		}

		fileName = fileName.ToLowerInvariant();
		string source = Path.Combine(folder, $"{CACHEFILE_PREFIX}{fileName}");
		bool refreshCache = File.GetLastWriteTimeUtc(source).AddHours(cacheHours ?? CacheHours) < timeProvider.GetUtcNow();

		if (!refreshCache || UseTestFiles) {
			if (File.Exists(source)) {
				jsonString = LoadFileFromCache(fileName);
			}
		}

		if (string.IsNullOrWhiteSpace(jsonString) && url is not null) {
			EnsureDefaultRequestHeaders(ttinfoId);
			HttpResponseMessage? response = await httpClient.GetAsync(url);
			if (response.IsSuccessStatusCode) {
				jsonString = await response.Content.ReadAsStringAsync();
				_ = SaveFileToCache(jsonString, fileName);
			//} else if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable) {
			//	jsonString = LoadFile(fileName);
			} else {
				jsonString = LoadFileFromCache(fileName);
			}
		}

		if (!string.IsNullOrWhiteSpace(jsonString)) {
			returnValue = JsonSerializer.Deserialize<T>(jsonString, JSON_SER_OPTIONS);
		}

		return returnValue;
	}
}
