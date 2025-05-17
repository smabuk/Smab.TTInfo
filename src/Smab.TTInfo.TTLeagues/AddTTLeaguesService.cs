using Microsoft.Extensions.DependencyInjection;

namespace Smab.TTInfo.TTLeagues;

/// <summary>
/// Provides extension methods for registering TTLeagues services in an <see cref="IServiceCollection"/>.
/// </summary>
/// <remarks>These extension methods simplify the configuration and registration of TTLeagues-related services,
/// including options binding and validation. Use these methods to add and configure the required services for TTLeagues
/// functionality in your application.</remarks>
public static class TTLeaguesServiceExtensions
{
	/// <summary>
	/// Adds the TTLeagues service and its associated configuration to the specified <see cref="IServiceCollection"/>.
	/// </summary>
	/// <remarks>This method registers the <see cref="TTLeaguesReader"/> service with a scoped lifetime and
	/// configures the  <see cref="TTLeaguesOptions"/> using the specified configuration section. The options are validated
	/// using  data annotations and are also validated at application startup.</remarks>
	/// <param name="services">The <see cref="IServiceCollection"/> to which the TTLeagues service will be added. Cannot be <see
	/// langword="null"/>.</param>
	/// <param name="configSectionName">The name of the configuration section to bind to the <see cref="TTLeaguesOptions"/>.  Must not be <see
	/// langword="null"/> or whitespace. Defaults to <c>TTINFO_OPTIONS_NAME</c>.</param>
	/// <returns>The updated <see cref="IServiceCollection"/> with the TTLeagues service registered.</returns>
	/// <exception cref="ArgumentException">Thrown if <paramref name="configSectionName"/> is <see langword="null"/> or consists only of whitespace.</exception>
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

	/// <summary>
	/// Adds the TTLeagues service to the specified <see cref="IServiceCollection"/> with the provided configuration
	/// options.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to which the TTLeagues service will be added. Cannot be <see
	/// langword="null"/>.</param>
	/// <param name="options">A delegate to configure the <see cref="TTLeaguesOptions"/> for the service.</param>
	/// <param name="configSectionName">The name of the configuration section to bind to <see cref="TTLeaguesOptions"/>. Defaults to "TTINFO_OPTIONS_NAME".</param>
	/// <returns>The same <see cref="IServiceCollection"/> instance, allowing for method chaining.</returns>
	public static IServiceCollection? AddTTLeaguesService(this IServiceCollection? services, Action<TTLeaguesOptions> options, string configSectionName = TTINFO_OPTIONS_NAME)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		_ = services.AddTTLeaguesService(configSectionName);
		_ = services.PostConfigure(options);

		return services;
	}
}
