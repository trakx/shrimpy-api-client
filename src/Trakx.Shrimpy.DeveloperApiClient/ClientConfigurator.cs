using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Trakx.Shrimpy.Core;
using Trakx.Shrimpy.Core.Utils;
using Trakx.Utils.Apis;

namespace Trakx.Shrimpy.DeveloperApiClient
{
    internal class ClientConfigurator : IClientConfigurator
    {
        private readonly IServiceProvider _serviceProvider;

        public ClientConfigurator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            ApiConfiguration = serviceProvider.GetService<IOptions<ShrimpyApiConfiguration>>()!.Value;
        }

        public ShrimpyApiConfiguration ApiConfiguration { get; }

        public ICredentialsProvider GetCredentialProvider(Type clientType)
        {
            switch (clientType.Name)
            {
                case nameof(MarketDataClient):
                //case nameof(HistoricalClient):
                    return new NoCredentialsProvider();
                default:
                    return _serviceProvider.GetService<IShrimpyCredentialsProvider>()!;
            }
        }
    }
}
