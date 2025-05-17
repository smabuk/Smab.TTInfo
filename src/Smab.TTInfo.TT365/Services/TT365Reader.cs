using System.Globalization;

namespace Smab.TTInfo.TT365.Services;

/// <summary>
/// Provides functionality to interact with the Table Tennis 365 (TT365) API, including caching and configuration
/// options.
/// </summary>
/// <remarks>This class is designed to facilitate communication with the TT365 API, allowing users to configure
/// caching behavior and toggle the use of test files for development purposes. It is a sealed class and cannot be
/// inherited.</remarks>
public sealed partial class TT365Reader(IOptions<TT365Options> options, HttpClient httpClient, TimeProvider timeProvider) : ITT365Reader
{
	private static readonly string                CACHEFILE_PREFIX      = "tt365_";
	private static readonly CultureInfo           GB_CULTURE            = new("en-GB");
	private static readonly string                TT365_COM             = "https://www.tabletennis365.com";
	private static readonly JsonSerializerOptions JSON_SER_OPTIONS      = new()
	{
		ReadCommentHandling         = JsonCommentHandling.Skip,
		PropertyNameCaseInsensitive = true,
	};

	public string CacheFolder  { get; set; } = options.Value.CacheFolder;
	public int    CacheHours   { get; set; } = options.Value.CacheHours;
	public bool   UseTestFiles { get; set; } = options.Value.UseTestFiles;
}
