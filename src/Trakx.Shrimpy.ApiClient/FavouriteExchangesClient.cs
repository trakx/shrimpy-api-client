using System.Collections.Generic;
using Trakx.Utils.Extensions;

namespace Trakx.Shrimpy.ApiClient;

public abstract class FavouriteExchangesClient : IFavouriteExchangesClient
{
    public IReadOnlyList<string> Top12ExchangeIds { get; }
    public string Top12ExchangeIdsAsCsv { get; }

    protected FavouriteExchangesClient(ClientConfigurator clientConfigurator)
    {
        ApiConfiguration = clientConfigurator.ApiConfiguration;

        Top12ExchangeIds = string.IsNullOrWhiteSpace(ApiConfiguration.FavouriteExchangesAsCsv)
            ? new List<string>
            {
                "binance", "binanceUs", "coinbasePro", "kraken", "kucoin", "huobiGlobal", "gemini", "gateio", "bittrex"
            }.AsReadOnly()
            : ApiConfiguration.FavouriteExchangesAsCsv.SplitCsvToLowerCaseDistinctList();

        Top12ExchangeIdsAsCsv = string.Join(",", Top12ExchangeIds);
    }

    public ShrimpyApiConfiguration ApiConfiguration { get; init; }
}
