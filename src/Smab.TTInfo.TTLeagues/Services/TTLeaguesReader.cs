using System.Globalization;
using Microsoft.Extensions.Options;

namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader : ITTLeaguesReader
{
	private static readonly string                CACHEFILE_PREFIX      = "ttl_";
	private static readonly CultureInfo           GB_CULTURE            = new("en-GB");
	private static readonly string                TTLEAGUES_API         = "https://ttleagues-api.azurewebsites.net/api/";
	private static readonly JsonSerializerOptions JSON_SER_OPTIONS      = new()
	{
		ReadCommentHandling         = JsonCommentHandling.Skip,
		PropertyNameCaseInsensitive = true,
	};

	private readonly HttpClient httpClient;
	private readonly TimeProvider timeProvider;

	public TTLeaguesReader(IOptions<TTLeaguesOptions> options, HttpClient httpClient, TimeProvider timeProvider)
	{
		CacheFolder  = options.Value.CacheFolder;
		CacheHours   = options.Value.CacheHours;
		UseTestFiles = options.Value.UseTestFiles;

		this.httpClient = httpClient;
		this.timeProvider = timeProvider;
		this.httpClient.BaseAddress = new Uri(TTLEAGUES_API);
	}

	public string CacheFolder  { get; set; }
	public int    CacheHours   { get; set; }
	public bool   UseTestFiles { get; set; }
}
