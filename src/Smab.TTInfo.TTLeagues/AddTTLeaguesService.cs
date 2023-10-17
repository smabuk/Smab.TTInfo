using Microsoft.Extensions.DependencyInjection;

namespace Smab.TTInfo.TTLeagues;
public static class TTLeaguesServiceExtensions
{
	public static IServiceCollection? AddTTLeaguesService(this IServiceCollection? services, string configSectionName = TTINFO_OPTIONS_NAME)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		if (string.IsNullOrWhiteSpace(configSectionName)) {
			throw new ArgumentException($"'{nameof(configSectionName)}' cannot be null or whitespace.", nameof(configSectionName));
		}

		_ = services.AddOptions<TTLeaguesOptions>()
			.BindConfiguration(configSectionName)
			.ValidateDataAnnotations()
			.ValidateOnStart();

		return services.AddScoped<TTLeaguesReader>();
	}

	public static IServiceCollection? AddTTLeaguesService(this IServiceCollection? services, Action<TTLeaguesOptions> options, string configSectionName = "TTInfo")
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		_ = services.AddTTLeaguesService(configSectionName);

		_ = services.PostConfigure(options);

		TTLeaguesOptions ttInfoOptions = new();
		options.Invoke(ttInfoOptions);

		return services;
	}
}
