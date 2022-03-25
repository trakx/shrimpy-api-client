using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Trakx.Shrimpy.ApiClient.Utils;
using Trakx.Utils.Apis;

namespace Trakx.Shrimpy.ApiClient;

public class ClientConfigurator
{
    public ShrimpyApiConfiguration ApiConfiguration { get; }
    private readonly IShrimpyCredentialsProvider _credentialsProvider;

    public ClientConfigurator(ShrimpyApiConfiguration apiConfiguration,
        IShrimpyCredentialsProvider credentialsProvider)
    {
        ApiConfiguration = apiConfiguration;
        _credentialsProvider = credentialsProvider;
    }

    public ICredentialsProvider GetCredentialProvider(Type clientType)
    {
        return clientType.Name switch
        {
            nameof(MarketDataClient) => new NoCredentialsProvider(),
            _ => _credentialsProvider
        };
    }
}