namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader
{
	public HttpClient CreateHttpClient(string ttinfoId)
	{
		ttinfoId = ttinfoId.ToLowerInvariant();

		HttpClient client = httpClientFactory.CreateClient();
		client.BaseAddress = new Uri("https://ttleagues-api.azurewebsites.net/api/");
		client.DefaultRequestHeaders.Add("Tenant", $"{ttinfoId}.ttleagues.com");

		return client;
	}
}
