namespace Smab.TTInfo.TTClubs.Services;

/// <summary>
/// Provides functionality to read and cache data from TTClubs.
/// </summary>
public sealed partial class TTClubsReader
{
	/// <summary>
	/// Loads JSON data asynchronously for a given type and key.
	/// </summary>
	/// <typeparam name="T">The type to deserialize the JSON data to.</typeparam>
	/// <param name="ttinfoId">The TTInfo identifier.</param>
	/// <param name="url">The URL to fetch the data from.</param>
	/// <param name="fileName">The file name for caching.</param>
	/// <param name="cacheFolder">The folder to use for caching. Defaults to the configured cache folder.</param>
	/// <param name="cacheHours">The number of hours to keep the cache valid. Defaults to the configured cache duration.</param>
	/// <returns>The deserialized object of type <typeparamref name="T"/>.</returns>
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
