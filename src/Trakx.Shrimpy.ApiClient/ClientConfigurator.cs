using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Trakx.Shrimpy.ApiClient.Utils;

namespace Trakx.Shrimpy.ApiClient
{
    internal class ClientConfigurator
    {
        private readonly IServiceProvider _serviceProvider;

        public ClientConfigurator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            ApiConfiguration = serviceProvider.GetService<IOptions<ShrimpyApiConfiguration>>().Value;
        }

        public ShrimpyApiConfiguration ApiConfiguration { get; }

        public ICredentialsProvider GetCredentialProvider(Type clientType)
        {
            switch (clientType.Name)
            {
                case nameof(MarketDataClient):
                //case nameof(ExchangesClient):
                    return new NoCredentialsProvider();
                default:
                    return _serviceProvider.GetService<ICredentialsProvider>();
            }
        }
    }
}