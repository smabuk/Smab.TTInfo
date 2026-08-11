using Microsoft.Extensions.DependencyInjection;

namespace Smab.TTInfo.Olop;

/// <summary>
/// Provides extension methods for registering Olop services in an <see cref="IServiceCollection"/>.
/// </summary>
/// <remarks>These extension methods simplify the configuration and registration of Olop-related services,
/// including options binding and validation. Use these methods to add and configure the required services for Olop
/// functionality in your application.</remarks>
public static class OlopServiceExtensions
{
	/// <summary>
	/// Adds the Olop service and its associated configuration to the specified <see cref="IServiceCollection"/>.
	/// </summary>
	/// <remarks>This method registers the <see cref="OlopJsInterop"/> service with a scoped lifetime and
	/// configures the  <see cref="OlopOptions"/> using the specified configuration section. The options are validated
	/// using  data annotations and are also validated at application startup.</remarks>
	/// <param name="services">The <see cref="IServiceCollection"/> to which the Olop service will be added. Cannot be <see
	/// langword="null"/>.</param>
	/// <param name="configSectionName">The name of the configuration section to bind to the <see cref="OlopOptions"/>.  Must not be <see
	/// langword="null"/> or whitespace. Defaults to <c>OLOP</c>.</param>
	/// <returns>The updated <see cref="IServiceCollection"/> with the Olop service registered.</returns>
	/// <exception cref="ArgumentException">Thrown if <paramref name="configSectionName"/> is <see langword="null"/> or consists only of whitespace.</exception>
	public static IServiceCollection? AddOlopService(this IServiceCollection? services, string configSectionName = "OLOP")
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		if (string.IsNullOrWhiteSpace(configSectionName)) {
			throw new ArgumentException($"'{nameof(configSectionName)}' cannot be null or whitespace.", nameof(configSectionName));
		}

		_ = services.AddOptions<OlopOptions>()
			.BindConfiguration(configSectionName)
			.ValidateDataAnnotations()
			.ValidateOnStart();

		return services.AddScoped<OlopJsInterop>();
	}

	/// <summary>
	/// Adds the Olop service to the specified <see cref="IServiceCollection"/> with the provided configuration
	/// options.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to which the Olop service will be added. Cannot be <see
	/// langword="null"/>.</param>
	/// <param name="options">A delegate to configure the <see cref="OlopOptions"/> for the service.</param>
	/// <param name="configSectionName">The name of the configuration section to bind to <see cref="OlopOptions"/>. Defaults to "OLOP".</param>
	/// <returns>The same <see cref="IServiceCollection"/> instance, allowing for method chaining.</returns>
	public static IServiceCollection? AddOlopService(this IServiceCollection? services, Action<OlopOptions> options, string configSectionName = "OLOP")
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		_ = services.AddOlopService(configSectionName);
		_ = services.PostConfigure(options);

		return services;
	}
}
