using Trakx.Shrimpy.ApiClient.Utils;

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
            nameof(MarketDataClient) => new NoCredentialProvider(),
            _ => _credentialsProvider
        };
    }
}