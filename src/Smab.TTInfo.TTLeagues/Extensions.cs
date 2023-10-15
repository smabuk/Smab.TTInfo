using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Smab.TTInfo.Shared.Models;

namespace Smab.TTInfo.TTLeagues;
public static class PlexInfoServerExtensions
{
	public static readonly int DEFAULT_DURATION = 60 * 24; // 24 hours
	private static TTInfoOptions _TTInfoOptions = new();

	public static WebApplicationBuilder? AddPlexInfoServer(this WebApplicationBuilder? builder)
	{
		ArgumentNullException.ThrowIfNull(builder, nameof(builder));

		_TTInfoOptions = builder.Configuration.GetSection(nameof(TTInfoOptions)).Get<TTInfoOptions>();

		builder.Services.Configure<TTInfoOptions>(builder.Configuration.GetSection(nameof(TTInfoOptions)));

		// Register the HttpClient for use in the controllers and services
		builder.Services.AddHttpClient<IPlexClient, PlexClient>()
		// The local Plex Server will not have a proper certificate so we have to ignore this
		.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
		{
			ClientCertificateOptions = ClientCertificateOption.Manual,
			ServerCertificateCustomValidationCallback =
			(httpRequestMessage, cert, certChain, policyErrors) =>
			{
				return true;
			}
		});

		return builder;
	}

	public static WebApplicationBuilder? AddPlexInfoServer(this WebApplicationBuilder? builder, Action<TTInfoOptions> options)
	{
		ArgumentNullException.ThrowIfNull(builder, nameof(builder));

		builder.AddPlexInfoServer();

		builder.Services.PostConfigure(options);

		TTInfoOptions TTInfoOptions = new();
		options.Invoke(TTInfoOptions);

		_TTInfoOptions.Server = TTInfoOptions.Server ?? _TTInfoOptions.Server;
		_TTInfoOptions.Token = TTInfoOptions.Token ?? _TTInfoOptions.Token;
		_TTInfoOptions.ThumbnailCacheDuration = TTInfoOptions.ThumbnailCacheDuration ?? _TTInfoOptions.ThumbnailCacheDuration;

		return builder;
	}

	public static IMvcBuilder ConfigurePlexInfoApis(this IMvcBuilder builder, Action<TTInfoOptions>? options = null)
	{
		ArgumentNullException.ThrowIfNull(nameof(builder));

		TTInfoOptions TTInfoOptions = new();
		options?.Invoke(TTInfoOptions);

		_TTInfoOptions.ThumbnailCacheDuration = TTInfoOptions.ThumbnailCacheDuration ?? _TTInfoOptions.ThumbnailCacheDuration;

		builder.AddMvcOptions(opt =>
			opt.CacheProfiles.Add("PlexInfoThumbnails",
			new()
			{
				// Multiply by 60 to convert from duration in minutes to seconds
				Duration = (_TTInfoOptions.ThumbnailCacheDuration ?? DEFAULT_DURATION) * 60
			})
		);

		return builder;
	}
}
