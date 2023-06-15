using Polly;
using Refit;

namespace Jordnaer.Client;

public static class WebAssemblyHostBuilderExtensions
{
    public static IServiceCollection AddRefitClient<TClient>(this IServiceCollection services, Uri baseAddress)
        where TClient : class
    {
        services.AddRefitClient<TClient>().ConfigureHttpClient(client =>
                client.BaseAddress = baseAddress)
            .AddTransientHttpErrorPolicy(policyBuilder =>
                policyBuilder.WaitAndRetryAsync(3, retryCount => TimeSpan.FromMilliseconds(50 * retryCount)))
            .AddTransientHttpErrorPolicy(policyBuilder =>
                policyBuilder.CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 3,
                    durationOfBreak: TimeSpan.FromSeconds(10)));

        return services;
    }

    public static IServiceCollection AddRefitClient<TClient>(this IServiceCollection services,
        string baseAddress) where TClient : class => services.AddRefitClient<TClient>(new Uri(baseAddress));
}
