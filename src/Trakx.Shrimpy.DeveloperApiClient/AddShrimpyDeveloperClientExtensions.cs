using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Trakx.Shrimpy.Core;

namespace Trakx.Shrimpy.DeveloperApiClient
{
    public static partial class AddShrimpyDeveloperClientExtensions
    {
        public static IServiceCollection AddDeveloperClients(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var config = configuration.GetSection(nameof(ShrimpyDevApiConfiguration)).Get<ShrimpyDevApiConfiguration>();
            serviceCollection.AddDeveloperClients(config);
            return serviceCollection;
        }

        public static IServiceCollection AddDeveloperClients(
            this IServiceCollection serviceCollection, ShrimpyDevApiConfiguration apiConfiguration)
        {
            serviceCollection.AddApiCredentialsProvider<ShrimpyDevApiConfiguration>();
            var options = Options.Create(apiConfiguration);
            serviceCollection.AddSingleton(options);
            serviceCollection.AddSingleton(s => new ClientConfigurator(s));
            AddClients(serviceCollection);
            return serviceCollection;
        }
    }
}
