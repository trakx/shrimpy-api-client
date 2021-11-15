using Microsoft.Extensions.DependencyInjection;

namespace Trakx.Shrimpy.ApiClient
{
    public static partial class AddShrimpyClientExtensions
    {
        public static void AddShrimpyClients(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(s => new ClientConfigurator(s));
            AddClients(serviceCollection);
        }
    }
}
