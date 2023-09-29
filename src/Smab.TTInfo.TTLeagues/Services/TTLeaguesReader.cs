using Microsoft.Extensions.Options;

using Smab.TTInfo.Shared.Models;

namespace Smab.TTInfo.TTLeagues.Services;
public sealed partial class TTLeaguesReader(IOptions<TTInfoOptions> options, IHttpClientFactory httpClientFactory) : ITTLeaguesReader
{
	private readonly static string CACHEFILE_PREFIX = "ttl_";
	
	public required string CacheFolder = options.Value.CacheFolder;
	public int CacheHours = options.Value.CacheHours;
	public bool UseTestFiles { get; set; } = options.Value.UseTestFiles;

	private readonly static System.Globalization.CultureInfo gbCulture = new("en-GB");
	private readonly static JsonSerializerOptions jsonSerializerOptions = new()
	{
		ReadCommentHandling = JsonCommentHandling.Skip,
		PropertyNameCaseInsensitive = true,
	};

}
