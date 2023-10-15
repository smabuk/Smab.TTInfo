using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Smab.TTInfo.TT365;
public static class TT365ServiceExtensions
{
	public static IServiceCollection AddTT365Service(this IServiceCollection? services, IConfiguration configuration)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		_ = services.Configure<TT365Options>(configuration.GetSection("TTInfo"));
		_ = services.AddScoped<ITT365Reader, TT365Reader>();

		return services;
	}

	public static IServiceCollection AddTT365Service(this IServiceCollection? services, IConfiguration configuration, Action<TT365Options> options)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		_ = services.AddTT365Service(configuration);

		_ = services.PostConfigure(options);

		TT365Options ttInfoOptions = new();
		options.Invoke(ttInfoOptions);

		return services;
	}
}
