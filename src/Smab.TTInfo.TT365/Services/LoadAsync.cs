using HtmlAgilityPack;

namespace Smab.TTInfo.TT365.Services;

/// <summary>
/// Provides methods for loading and caching data from remote sources or local cache for TT365.
/// </summary>
public sealed partial class TT365Reader
{
	/// <summary>
	/// Loads data of type <typeparamref name="T"/> from cache or remote source, optionally using a cache file and folder.
	/// </summary>
	/// <typeparam name="T">The type to deserialize the data to.</typeparam>
	/// <param name="ttinfoId">The TT365 league identifier.</param>
	/// <param name="url">The URL to fetch data from if not found in cache.</param>
	/// <param name="fileName">The cache file name.</param>
	/// <param name="cacheFolder">The cache folder path.</param>
	/// <param name="cacheHours">The cache expiration in hours.</param>
	/// <returns>The deserialized object or default if not found.</returns>
	public async Task<T?> LoadAsync<T>(string ttinfoId, string? url, string fileName = "", string? cacheFolder = null, int? cacheHours = null)
	{
		bool useCache = !string.IsNullOrWhiteSpace(fileName);
		string? contentString = null;
		T? returnValue = default;

		if (useCache) {
			string folder = cacheFolder ?? CacheFolder;

			if (!Directory.Exists(folder)) {
				_ = Directory.CreateDirectory(folder);
			}

			fileName = fileName.ToLowerInvariant();
			string source = Path.Combine(folder, $"{CACHEFILE_PREFIX}{fileName}");
			bool refreshCache = File.GetLastWriteTimeUtc(source).AddHours(cacheHours ?? CacheHours) < timeProvider.GetUtcNow();

			if (!refreshCache || UseTestFiles) {
				if (File.Exists(source)) {
					contentString = LoadFileFromCache(fileName);
				}
			}
		}

		if (string.IsNullOrWhiteSpace(contentString) && url is not null) {
			EnsureBaseAddress(ttinfoId);
			HttpResponseMessage? response = await httpClient.GetAsync(url);
			if (response.IsSuccessStatusCode) {
				contentString = await response.Content.ReadAsStringAsync();
				if (useCache) {
					_ = SaveFileToCache(contentString, fileName);
				}
			//} else if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable) {
			//	contentString = LoadFileFromCache(fileName);
			} else {
				if (useCache) {
					contentString = LoadFileFromCache(fileName);
				}
			}
		}

		if (typeof(T).Name == "HtmlDocument" && contentString is not null) {
			HtmlDocument rv = new();
			rv.LoadHtml(contentString);
			//rv.Save(source);
			returnValue = (T?)Convert.ChangeType(rv, typeof(T));
		} else if (!string.IsNullOrWhiteSpace(contentString)) {
			returnValue = JsonSerializer.Deserialize<T>(contentString, JSON_SER_OPTIONS);
		}

		return returnValue;
	}

	/// <summary>
	/// Ensures the base address of the HTTP client is set for the specified TT365 league.
	/// </summary>
	/// <param name="ttinfoId">The TT365 league identifier.</param>
	private void EnsureBaseAddress(string ttinfoId)
	{
		httpClient.BaseAddress ??= new Uri($"{TT365_COM}/{ttinfoId}/");
	}
}
