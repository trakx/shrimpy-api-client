using System.Collections.Generic;

namespace Trakx.Shrimpy.ApiClient;

public abstract class FavouriteExchangesClient : IFavouriteExchangesClient
{
    public IReadOnlyList<string> Top12ExchangeIds { get; }
    public string Top12ExchangeIdsAsCsv { get; }

    protected FavouriteExchangesClient(ClientConfigurator clientConfigurator)
    {
        ApiConfiguration = clientConfigurator.ApiConfiguration;
        Top12ExchangeIds = ApiConfiguration.FavouriteExchanges?.Count > 0
            ? ApiConfiguration.FavouriteExchanges!.AsReadOnly()
            : new List<string>
            {
                "bitstamp", "bittrex", "poloniex", "kraken", "bitfinex", "coinbasepro",
                "itbi", "gemini", "binance", "bfly", "cflr", "huobiglobal"
            }.AsReadOnly();

        Top12ExchangeIdsAsCsv = string.Join(",", Top12ExchangeIds);
    }

    public ShrimpyApiConfiguration ApiConfiguration { get; init; }
}
