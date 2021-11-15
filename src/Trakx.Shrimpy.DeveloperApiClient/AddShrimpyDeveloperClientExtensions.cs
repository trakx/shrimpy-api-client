using Microsoft.Extensions.DependencyInjection;

namespace Trakx.Shrimpy.DeveloperApiClient
{
    public static partial class AddShrimpyDeveloperClientExtensions
    {
        public static void AddDeveloperClients(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(s => new ClientConfigurator(s));
            AddClients(serviceCollection);
        }
    }
}
