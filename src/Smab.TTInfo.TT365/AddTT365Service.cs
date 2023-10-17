using Microsoft.Extensions.DependencyInjection;

namespace Smab.TTInfo.TT365;
public static class TT365ServiceExtensions
{
	public static IServiceCollection AddTT365Service(this IServiceCollection? services, string configSectionName = TTINFO_OPTIONS_NAME)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));
		
		if (string.IsNullOrWhiteSpace(configSectionName)) {
			throw new ArgumentException($"'{nameof(configSectionName)}' cannot be null or whitespace.", nameof(configSectionName));
		}

		_ = services.AddOptions<TT365Options>()
			.BindConfiguration(configSectionName)
			.ValidateDataAnnotations()
			.ValidateOnStart();

		return services.AddScoped<ITT365Reader, TT365Reader>();
	}

	public static IServiceCollection AddTT365Service(this IServiceCollection? services, Action<TT365Options> options, string configSectionName = TTINFO_OPTIONS_NAME)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		_ = services.AddTT365Service(configSectionName);

		_ = services.PostConfigure(options);

		TT365Options ttInfoOptions = new();
		options.Invoke(ttInfoOptions);

		return services;
	}
}
