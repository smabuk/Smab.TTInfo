namespace Smab.TTInfo.TTLeagues.Services;

/// <summary>
/// Ensures default request headers are set for HTTP requests.
/// </summary>
public sealed partial class TTLeaguesReader
{
	internal void EnsureDefaultRequestHeaders(string ttinfoId)
	{
		if (!httpClient.DefaultRequestHeaders.Any(dr => dr.Key == "Tenant")) {
			httpClient.DefaultRequestHeaders.Add("Tenant", $"{ttinfoId}.ttleagues.com");
		}
	}
}
