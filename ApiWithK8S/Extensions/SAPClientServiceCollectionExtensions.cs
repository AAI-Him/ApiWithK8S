using ConsoleTest;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ApiWithK8S.Extensions
{
    public static class SAPClientServiceCollectionExtensions
    {
        public static IServiceCollection AddSAPClient(this IServiceCollection services, Action<SAPClientOptions> action)
        {
            services.TryAddScoped<ISAPClientFactory, SAPClientFactory>();
            services.Configure(action);
            return services;
        }
    }
}
