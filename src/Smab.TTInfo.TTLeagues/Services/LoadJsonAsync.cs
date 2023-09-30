namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	public async Task<T?> LoadJsonAsync<T>(string leagueId, string? url, string fileName, int? cacheHours = null)
	{
		string? jsonString = null;
		T? returnValue = default;

		if (!Directory.Exists(CacheFolder)) {
			_ = Directory.CreateDirectory(CacheFolder);
		}

		string source = Path.Combine(CacheFolder, $"{CACHEFILE_PREFIX}{fileName}");
		bool refreshCache = File.GetLastWriteTimeUtc(source).AddHours(cacheHours ?? CacheHours) < DateTime.UtcNow;

		if (!refreshCache || UseTestFiles || url is null) {
			if (File.Exists(source)) {
				jsonString = LoadFile(fileName);
			}
		}

		if (string.IsNullOrWhiteSpace(jsonString) && url is not null) {
			using HttpClient client = CreateHttpClient(leagueId);
			HttpResponseMessage? response = await client.GetAsync(url);
			if (response.IsSuccessStatusCode) {
				jsonString = await response.Content.ReadAsStringAsync();
				_ = SaveFile(jsonString, fileName);
			//} else if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable) {
			//	jsonString = LoadFile(fileName);
			} else {
				jsonString = LoadFile(fileName);
			}
		}

		if (!string.IsNullOrWhiteSpace(jsonString)) {
			returnValue = JsonSerializer.Deserialize<T>(jsonString, jsonSerializerOptions);
		}

		return returnValue;
	}
}
