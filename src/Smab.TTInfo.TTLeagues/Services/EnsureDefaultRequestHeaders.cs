namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	internal void EnsureDefaultRequestHeaders(string ttinfoId)
	{
		if (!httpClient.DefaultRequestHeaders.Any(dr => dr.Key == "Tenant")) {
			httpClient.DefaultRequestHeaders.Add("Tenant", $"{ttinfoId}.ttleagues.com");
		}
	}
}
