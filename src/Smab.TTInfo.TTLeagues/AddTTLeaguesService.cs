using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Smab.TTInfo.TTLeagues;
public static class TTLeaguesServiceExtensions
{
	public static IServiceCollection? AddTTLeaguesService(this IServiceCollection? services, IConfiguration configuration)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		_ = services.Configure<TTLeaguesOptions>(configuration.GetSection("TTInfo"));
		_ = services.AddScoped<TTLeaguesReader>();

		return services;
	}

	public static IServiceCollection? AddTTLeaguesService(this IServiceCollection? services, IConfiguration configuration, Action<TTLeaguesOptions> options)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		_ = services.AddTTLeaguesService(configuration);

		_ = services.PostConfigure(options);

		TTLeaguesOptions ttInfoOptions = new();
		options.Invoke(ttInfoOptions);

		return services;
	}
}
