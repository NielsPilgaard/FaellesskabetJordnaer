using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace Jordnaer.Shared;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddDataForsyningenClient(this IServiceCollection services)
	{
		services.AddRefitClient<IDataForsyningenClient>()
				.ConfigureHttpClient((provider, client) =>
				{
					var dataForsyningenOptions = provider.GetRequiredService<IOptions<DataForsyningenOptions>>().Value;

					client.BaseAddress = new Uri(dataForsyningenOptions.BaseUrl);
				})
				.AddStandardResilienceHandler();

		return services;
	}
}
