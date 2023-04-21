using System.Text.Json;
using Serilog;

namespace Trakx.Shrimpy.ApiClient.Tests.Integration;

public class MarketDataClientTests : ShrimpyClientTestsBase
{
    private readonly IMarketDataClient _marketDataClient;

    public MarketDataClientTests(ShrimpyApiFixture apiFixture, ITestOutputHelper output) : base(apiFixture, output)
    {
        _marketDataClient = ServiceProvider.GetRequiredService<IMarketDataClient>();
    }

    [Fact]
    public async Task GetTicker_should_return_tickers_for_all_exchanges()
    {
        var tasks = Enum.GetValues(typeof(Exchange)).Cast<Exchange>().Select(async exchange =>
        {
            try
            {
                var tickers = await _marketDataClient.GetTickerAsync(exchange);
                tickers.Content.Count.Should().BeGreaterThan(10);
                var knownSymbols = tickers.Content.Select(t => t.Symbol);
                Logger.Information("Exchange {exchange} has tickers:" +
                                   Environment.NewLine + "{tickers}", exchange,
                    string.Join(",", knownSymbols));
            }
            catch (Exception exception)
            {
                Logger.Warning(exception, "Failed to get tickers for exchange {exchange}", exchange);
            }
        }).ToArray();

        await Task.WhenAll(tasks);
    }

    [Fact]
    public void ListExchanges()
    {
        var exchanges = Enum.GetValues(typeof(Exchange)).Cast<Exchange>().Select(e => e.ToString());
        Logger.Information(string.Join(", ", exchanges));
        exchanges.Count().Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetTicker_should_return_all_tickers_including_OKB_from_okex()
    {
        var tickers = await _marketDataClient.GetTickerAsync(Exchange.Okex);
        tickers.Content.Count.Should().BeGreaterThan(10);
        var knownSymbols = tickers.Content.Select(t => t.Name).ToList();
        knownSymbols.Should().Contain("OKB");
        Logger.Information(string.Join(",", knownSymbols));
    }

    [Theory]
    [InlineData("ELF")]
    [InlineData("HT")]
    public async Task GetTicker_should_return_price_from_exchanges(string symbol)
    {
        var tasks = Enum.GetValues(typeof(Exchange)).Cast<Exchange>().Select(async exchange =>
        {
            try
            {
                var tickers = await _marketDataClient.GetTickerAsync(exchange);
                tickers.Content.Count.Should().BeGreaterThan(1);
                var knownSymbols = tickers.Content.Where(t => t.Symbol == symbol).ToList();
                knownSymbols.Count.Should().BeLessOrEqualTo(1);
                var ticker = knownSymbols.SingleOrDefault();
                if (ticker is null) Log.Information("{exchange} doesn't have ticker {symbol}", exchange, symbol);
                else
                    Logger.Information("{exchange} has the following for {symbol} ticker {ticker}", exchange,
                        symbol, JsonSerializer.Serialize(ticker));
            }
            catch (Exception exception)
            {
                Logger.Warning(exception, "Failed to get tickers for exchange {exchange}", exchange);
            }
        }).ToArray();

        await Task.WhenAll(tasks);
    }

}
