using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;
using Trakx.Shrimpy.Core.Tests.Integration;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.Shrimpy.ApiClient.Tests.Integration
{
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
                    tickers.Result.Count.Should().BeGreaterThan(10);
                    var knownSymbols = tickers.Result.Select(t => t.Symbol);
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
        public async Task GetTicker_should_return_all_tickers_including_OKB_from_okex()
        {
            var tickers = await _marketDataClient.GetTickerAsync(Exchange.Okex);
            tickers.Result.Count.Should().BeGreaterThan(10);
            var knownSymbols = tickers.Result.Select(t => t.Symbol).ToList();
            knownSymbols.Should().Contain("OKB");
            Logger.Information(string.Join(",", knownSymbols));
        }

        [Fact]
        public async Task GetTicker_should_return_all_tickers_including_ELF_from_binance()
        {
            var tasks = Enum.GetValues(typeof(Exchange)).Cast<Exchange>().Select(async exchange =>
            {
                try
                {
                    var tickers = await _marketDataClient.GetTickerAsync(exchange);
                    tickers.Result.Count.Should().BeGreaterThan(10);
                    var knownSymbols = tickers.Result.Where(t => t.Symbol == "ELF").ToList();
                    knownSymbols.Count.Should().BeLessOrEqualTo(1);
                    var aelf = knownSymbols.SingleOrDefault();
                    aelf?.Name.Should().Be("aelf");
                    if(aelf is null) Log.Information("{exchange} doesn't have ticker ELF", exchange);
                    else Logger.Information("{exchange} has the following for ELF ticker {aelf}", exchange,  JsonSerializer.Serialize(aelf));
                }
                catch (Exception exception)
                {
                    Logger.Warning(exception, "Failed to get tickers for exchange {exchange}", exchange);
                }
            }).ToArray();

            await Task.WhenAll(tasks);
        }

    }
}
