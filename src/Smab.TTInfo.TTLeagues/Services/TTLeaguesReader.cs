using System.Globalization;
using Microsoft.Extensions.Options;

using Smab.TTInfo.Shared.Models;

namespace Smab.TTInfo.TTLeagues.Services;

public sealed partial class TTLeaguesReader(IOptions<TTInfoOptions> options, IHttpClientFactory httpClientFactory) : ITTLeaguesReader
{
	private static readonly string                CACHEFILE_PREFIX      = "ttl_";
	private static readonly CultureInfo           GB_CULTURE            = new("en-GB");
	private static readonly JsonSerializerOptions jsonSerializerOptions = new()
	{
		ReadCommentHandling         = JsonCommentHandling.Skip,
		PropertyNameCaseInsensitive = true,
	};

	public string CacheFolder  { get; set; } = options.Value.CacheFolder;
	public int    CacheHours   { get; set; } = options.Value.CacheHours;
	public bool   UseTestFiles { get; set; } = options.Value.UseTestFiles;
}
