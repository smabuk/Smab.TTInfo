using Smab.TTInfo.Shared.Models;

using System.Globalization;

namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader(IOptions<TTInfoOptions> options) : ITT365Reader
{
	private static readonly string                CACHEFILE_PREFIX      = "tt365_";
	private static readonly string                TT365_COM             = $"https://www.tabletennis365.com";
	private static readonly CultureInfo           GB_CULTURE            = new("en-GB");
	private static readonly JsonSerializerOptions JSON_SER_OPTIONS      = new()
	{
		ReadCommentHandling         = JsonCommentHandling.Skip,
		PropertyNameCaseInsensitive = true,
	};

	public string CacheFolder  { get; set; } = options.Value.CacheFolder;
	public int    CacheHours   { get; set; } = options.Value.CacheHours;
	public bool   UseTestFiles { get; set; } = options.Value.UseTestFiles;
}
