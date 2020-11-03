using Microsoft.Extensions.Options;

namespace Trakx.Shrimpy.ApiClient
{
    internal class ClientConfigurator
    {
        public ClientConfigurator(IOptions<ShrimpyApiConfiguration> configuration, I)
        {
            ApiConfiguration = configuration.Value;
        }

        public ShrimpyApiConfiguration ApiConfiguration { get; }
    }
}