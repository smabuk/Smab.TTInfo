using System.Globalization;

using Microsoft.Extensions.Options;

namespace Smab.TTInfo.TTClubs.Services;

internal interface ITTClubsReader
{
}


/// <summary>
/// Provides methods to read and manage TTClubs data, including caching and API access.
/// </summary>
public sealed partial class TTClubsReader : ITTClubsReader
{
	/// <summary>
	/// Prefix for cache files.
	/// </summary>
	private static readonly string CACHEFILE_PREFIX = "ttclubs_";
	/// <summary>
	/// The culture info for Great Britain.
	/// </summary>
	private static readonly CultureInfo GB_CULTURE = new("en-GB");
	/// <summary>
	/// The base URL for the TTClubs API.
	/// </summary>
	private static readonly string TTClubs_API = "https://ttleagues-api.azurewebsites.net/api/";
	/// <summary>
	/// The default JSON serializer options for TTClubs data.
	/// </summary>
	private static readonly JsonSerializerOptions JSON_SER_OPTIONS = new()
	{
		ReadCommentHandling = JsonCommentHandling.Skip,
		PropertyNameCaseInsensitive = true,
	};

	private readonly HttpClient httpClient;
	private readonly TimeProvider timeProvider;

	/// <summary>
	/// Initializes a new instance of the <see cref="TTClubsReader"/> class.
	/// </summary>
	/// <param name="options">The TTClubs options.</param>
	/// <param name="httpClient">The HTTP client for API requests.</param>
	/// <param name="timeProvider">The time provider for cache management.</param>
	public TTClubsReader(IOptions<TTClubsOptions> options, HttpClient httpClient, TimeProvider timeProvider)
	{
		CacheFolder = options.Value.CacheFolder;
		CacheHours = options.Value.CacheHours;
		UseTestFiles = options.Value.UseTestFiles;

		this.httpClient = httpClient;
		this.timeProvider = timeProvider;
		this.httpClient.BaseAddress = new Uri(TTClubs_API);
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
