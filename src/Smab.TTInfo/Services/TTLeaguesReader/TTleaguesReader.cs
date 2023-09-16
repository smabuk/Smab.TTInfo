using Microsoft.Extensions.Options;

using Smab.TTInfo.Services;

namespace Smab.TTInfo;
public sealed partial class TTLeaguesReader(IOptions<TTInfoOptions> options)
{
	public required string CacheFolder = options.Value.CacheFolder;
	public int CacheHours = options.Value.CacheHours;
	public bool UseTestFiles { get; set; } = options.Value.UseTestFiles;

	private readonly static System.Globalization.CultureInfo gbCulture = new("en-GB");
}
