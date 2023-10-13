namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader
{
	public HttpClient CreateHttpClient(string ttinfoId)
	{
		//HttpClient client = httpClientFactory.CreateClient();
		HttpClient client = new()
		{
			BaseAddress = new Uri($"{TT365_COM}/{ttinfoId}/")
		};

		return client;
	}
}
