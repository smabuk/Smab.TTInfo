namespace Smab.TTInfo.TTClubs.Services;

/// <summary>
/// Ensures default request headers are set for HTTP requests.
/// </summary>
public sealed partial class TTClubsReader
{
	internal void EnsureDefaultRequestHeaders(string ttinfoId)
	{
		if (!httpClient.DefaultRequestHeaders.Any(dr => dr.Key == "Tenant")) {
			httpClient.DefaultRequestHeaders.Add("Tenant", $"{ttinfoId}.ttclubs.co.uk");
		}
	}
}
