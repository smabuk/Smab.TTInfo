﻿using System.Globalization;

namespace Smab.TTInfo.TT365.Services;

public sealed partial class TT365Reader : ITT365Reader
{
	private static readonly string                CACHEFILE_PREFIX      = "tt365_";
	private static readonly CultureInfo           GB_CULTURE            = new("en-GB");
	private static readonly string                TT365_COM             = "https://www.tabletennis365.com";
	private static readonly JsonSerializerOptions JSON_SER_OPTIONS      = new()
	{
		ReadCommentHandling         = JsonCommentHandling.Skip,
		PropertyNameCaseInsensitive = true,
	};
	
	private readonly HttpClient httpClient;
	private readonly TimeProvider timeProvider;

	public TT365Reader(IOptions<TT365Options> options, HttpClient httpClient, TimeProvider timeProvider)
	{
		CacheFolder = options.Value.CacheFolder;
		CacheHours = options.Value.CacheHours;
		UseTestFiles = options.Value.UseTestFiles;

		this.httpClient = httpClient;
		this.timeProvider = timeProvider;
	}

	public string CacheFolder  { get; set; }
	public int    CacheHours   { get; set; }
	public bool   UseTestFiles { get; set; }
}
