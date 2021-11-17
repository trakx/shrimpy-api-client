using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Trakx.Shrimpy.Core;

namespace Trakx.Shrimpy.ApiClient
{
    public static partial class AddShrimpyClientExtensions
    {
        public static IServiceCollection AddShrimpyClients(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var config = configuration.GetSection(nameof(ShrimpyApiConfiguration)).Get<ShrimpyApiConfiguration>();
            serviceCollection.AddShrimpyClients(config);
            return serviceCollection;
        }

        public static IServiceCollection AddShrimpyClients(
            this IServiceCollection serviceCollection, ShrimpyApiConfiguration apiConfiguration)
        {
            serviceCollection.AddApiCredentialsProvider<ShrimpyApiConfiguration>();
            var options = Options.Create(apiConfiguration);
            serviceCollection.AddSingleton(options);
            serviceCollection.AddSingleton(s => new ClientConfigurator(s));
            AddClients(serviceCollection);
            return serviceCollection;
        }
    }
}
