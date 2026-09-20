using System.Globalization;

using HtmlAgilityPack;

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
	private string CACHEFILE_PREFIX      => WebsiteVersion == TT365WebsiteVersion.Original ? "tt365_" : "tt365_new2026_";
	private string TT365_COM             => $"""https://www.tabletennis365.com""";
	//private string TT365_COM             => $"""https://{(WebsiteVersion == TT365WebsiteVersion.Original ? "www" : "beta")}.tabletennis365.com""";
	private static readonly CultureInfo    GB_CULTURE            = new("en-GB");
	public static readonly TimeOnly        DEFAULT_START_TIME    = new(19, 30);
	private readonly JsonSerializerOptions JSON_SER_OPTIONS      = new()
	{
		ReadCommentHandling         = JsonCommentHandling.Skip,
		PropertyNameCaseInsensitive = true,
	};
	private static readonly HtmlNodeCollection EMPTY_NODE_COLLECTION = new(HtmlNode.CreateNode("<div></div>"));


	public string CacheFolder  { get; set; } = options.Value.CacheFolder;
	public int    CacheHours   { get; set; } = options.Value.CacheHours;
	public bool   UseTestFiles { get; set; } = options.Value.UseTestFiles;
	public TT365WebsiteVersion WebsiteVersion => options.Value.GetTT365WebsiteVersion();
}
