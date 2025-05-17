using Microsoft.Extensions.DependencyInjection;

namespace Smab.TTInfo.TT365;
public static class TT365ServiceExtensions
{
	/// <summary>
	/// Adds the TT365 service and its dependencies to the specified <see cref="IServiceCollection"/>.
	/// </summary>
	/// <remarks>This method registers the <see cref="ITT365Reader"/> implementation, <see cref="TT365Reader"/>, as
	/// a scoped service. It also configures the <see cref="TT365Options"/> by binding it to the specified configuration
	/// section and validates the options using data annotations and startup validation.</remarks>
	/// <param name="services">The <see cref="IServiceCollection"/> to which the TT365 service will be added. Cannot be <see langword="null"/>.</param>
	/// <param name="configSectionName">The name of the configuration section to bind to the <see cref="TT365Options"/>.  Must not be <see
	/// langword="null"/> or whitespace. Defaults to <c>TTINFO_OPTIONS_NAME</c>.</param>
	/// <returns>The updated <see cref="IServiceCollection"/> with the TT365 service registered.</returns>
	/// <exception cref="ArgumentException">Thrown if <paramref name="configSectionName"/> is <see langword="null"/> or consists only of whitespace.</exception>
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

	/// <summary>
	/// Adds the TT365 service to the specified <see cref="IServiceCollection"/> with the provided configuration options.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to which the TT365 service will be added. Cannot be <see langword="null"/>.</param>
	/// <param name="options">A delegate to configure the <see cref="TT365Options"/> for the TT365 service.</param>
	/// <param name="configSectionName">The name of the configuration section to bind to the <see cref="TT365Options"/>. Defaults to "TTINFO_OPTIONS_NAME".</param>
	/// <returns>The updated <see cref="IServiceCollection"/> with the TT365 service added.</returns>
	public static IServiceCollection AddTT365Service(this IServiceCollection? services, Action<TT365Options> options, string configSectionName = TTINFO_OPTIONS_NAME)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		_ = services.AddTT365Service(configSectionName);
		_ = services.PostConfigure(options);

		return services;
	}
}
