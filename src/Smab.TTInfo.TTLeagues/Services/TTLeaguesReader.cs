using System.Globalization;
using Microsoft.Extensions.Options;

namespace Smab.TTInfo.TTLeagues.Services;

/// <summary>
/// Provides methods to read and manage TTLeagues data, including caching and API access.
/// </summary>
public sealed partial class TTLeaguesReader : ITTLeaguesReader
{
	/// <summary>
	/// Prefix for cache files.
	/// </summary>
	private static readonly string CACHEFILE_PREFIX = "ttl_";
	/// <summary>
	/// The culture info for Great Britain.
	/// </summary>
	private static readonly CultureInfo GB_CULTURE = new("en-GB");
	/// <summary>
	/// The base URL for the TTLeagues API.
	/// </summary>
	private static readonly string TTLEAGUES_API = "https://ttleagues-api.azurewebsites.net/api/";
	/// <summary>
	/// The default JSON serializer options for TTLeagues data.
	/// </summary>
	private static readonly JsonSerializerOptions JSON_SER_OPTIONS = new()
	{
		ReadCommentHandling = JsonCommentHandling.Skip,
		PropertyNameCaseInsensitive = true,
	};

	private readonly HttpClient httpClient;
	private readonly TimeProvider timeProvider;

	/// <summary>
	/// Initializes a new instance of the <see cref="TTLeaguesReader"/> class.
	/// </summary>
	/// <param name="options">The TTLeagues options.</param>
	/// <param name="httpClient">The HTTP client for API requests.</param>
	/// <param name="timeProvider">The time provider for cache management.</param>
	public TTLeaguesReader(IOptions<TTLeaguesOptions> options, HttpClient httpClient, TimeProvider timeProvider)
	{
		CacheFolder = options.Value.CacheFolder;
		CacheHours = options.Value.CacheHours;
		UseTestFiles = options.Value.UseTestFiles;

		this.httpClient = httpClient;
		this.timeProvider = timeProvider;
		this.httpClient.BaseAddress = new Uri(TTLEAGUES_API);
	}

	/// <summary>
	/// Gets or sets the cache folder path.
	/// </summary>
	public string CacheFolder { get; set; }
	/// <summary>
	/// Gets or sets the cache duration in hours.
	/// </summary>
	public int CacheHours { get; set; }
	/// <summary>
	/// Gets or sets a value indicating whether to use test files.
	/// </summary>
	public bool UseTestFiles { get; set; }
}
