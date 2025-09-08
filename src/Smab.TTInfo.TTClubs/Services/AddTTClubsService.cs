using Microsoft.Extensions.DependencyInjection;

namespace Smab.TTInfo.TTClubs.Services;

/// <summary>
/// Provides extension methods for registering TTClubs services in an <see cref="IServiceCollection"/>.
/// </summary>
/// <remarks>These extension methods simplify the configuration and registration of TTClubs-related services,
/// including options binding and validation. Use these methods to add and configure the required services for TTClubs
/// functionality in your application.</remarks>
public static class TTClubsServiceExtensions
{
	/// <summary>
	/// Adds the TTClubs service and its associated configuration to the specified <see cref="IServiceCollection"/>.
	/// </summary>
	/// <remarks>This method registers the <see cref="TTClubsReader"/> service with a scoped lifetime and
	/// configures the  <see cref="TTClubsOptions"/> using the specified configuration section. The options are validated
	/// using  data annotations and are also validated at application startup.</remarks>
	/// <param name="services">The <see cref="IServiceCollection"/> to which the TTClubs service will be added. Cannot be <see
	/// langword="null"/>.</param>
	/// <param name="configSectionName">The name of the configuration section to bind to the <see cref="TTClubsOptions"/>.  Must not be <see
	/// langword="null"/> or whitespace. Defaults to <c>TTINFO_OPTIONS_NAME</c>.</param>
	/// <returns>The updated <see cref="IServiceCollection"/> with the TTClubs service registered.</returns>
	/// <exception cref="ArgumentException">Thrown if <paramref name="configSectionName"/> is <see langword="null"/> or consists only of whitespace.</exception>
	public static IServiceCollection? AddTTClubsService(this IServiceCollection? services, string configSectionName = "TTInfo")
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		if (string.IsNullOrWhiteSpace(configSectionName)) {
			throw new ArgumentException($"'{nameof(configSectionName)}' cannot be null or whitespace.", nameof(configSectionName));
		}

		_ = services.AddOptions<TTClubsOptions>()
			.BindConfiguration(configSectionName)
			.ValidateDataAnnotations()
			.ValidateOnStart();

		return services.AddScoped<TTClubsReader>();
	}

	/// <summary>
	/// Adds the TTClubs service to the specified <see cref="IServiceCollection"/> with the provided configuration
	/// options.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to which the TTClubs service will be added. Cannot be <see
	/// langword="null"/>.</param>
	/// <param name="options">A delegate to configure the <see cref="TTClubsOptions"/> for the service.</param>
	/// <param name="configSectionName">The name of the configuration section to bind to <see cref="TTClubsOptions"/>. Defaults to "TTINFO_OPTIONS_NAME".</param>
	/// <returns>The same <see cref="IServiceCollection"/> instance, allowing for method chaining.</returns>
	public static IServiceCollection? AddTTClubsService(this IServiceCollection? services, Action<TTClubsOptions> options, string configSectionName = "TTInfo")
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		_ = services.AddTTClubsService(configSectionName);
		_ = services.PostConfigure(options);

		return services;
	}
}
