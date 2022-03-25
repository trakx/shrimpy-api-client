using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Trakx.Shrimpy.ApiClient.Utils;
using Trakx.Utils.DateTimeHelpers;

namespace Trakx.Shrimpy.ApiClient
{
    public static partial class AddShrimpyClientExtensions
    {
        public static IServiceCollection AddShrimpyClient(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var config = configuration.GetSection(nameof(ShrimpyApiConfiguration)).Get<ShrimpyApiConfiguration>();
            serviceCollection.AddShrimpyClient(config);
            return serviceCollection;
        }

        public static IServiceCollection AddShrimpyClient(
            this IServiceCollection serviceCollection, ShrimpyApiConfiguration apiConfiguration)
        {
            serviceCollection.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            serviceCollection.AddSingleton<IShrimpyCredentialsProvider, ApiKeyCredentialsProvider>();
            serviceCollection.AddSingleton(apiConfiguration);
            serviceCollection.AddSingleton<ClientConfigurator>();
            AddClients(serviceCollection);
            return serviceCollection;
        }
    }
}
