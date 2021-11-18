using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Trakx.Shrimpy.Core.Tests.Integration;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.Shrimpy.DeveloperApiClient.Tests.Integration
{
    public sealed class HistoricalClientTests : ShrimpyClientTestsBase
    {
        private readonly IHistoricalClient _historicalClient;
        public HistoricalClientTests(ShrimpyApiFixture apiFixture, ITestOutputHelper output) : base(apiFixture, output)
        {
            _historicalClient = ServiceProvider.GetRequiredService<IHistoricalClient>();
        }

        [Fact]
        public async Task GetHistoricalCandlesAsync_should_return_results()
        {
            var start = DateTimeOffset.Parse("2021-01-01z");
            var end = start.AddDays(2);
            var candles = (await _historicalClient
                .GetHistoricalCandlesAsync(Exchange.Bittrex, "LTC", "BTC", start, end, 100, Interval._1d))
                .Result;
            var first = candles.First();

            Logger.Information("Found candle: {candle}",
                JsonSerializer.Serialize(first));
        }

        [Fact]
        public async Task GetHistoricalInstrumentsAsync_should_return_results()
        {
            var instruments = (await _historicalClient
                    .GetHistoricalInstrumentsAsync(Exchange.Binance, "BTC"))
                .Result;
            var usd = instruments
                .Where(i => i.QuoteTradingSymbol.Contains("usd", StringComparison.InvariantCultureIgnoreCase));

            Logger.Information("Found instruments : {usd}",
                JsonSerializer.Serialize(usd));
        }


        [Fact]
        public async Task GetHistoricalCountAsync_should_return_results()
        {
            var start = DateTimeOffset.Parse("2020-10-28");
            var end = DateTimeOffset.Parse("2021-11-18");
            var count = (await _historicalClient
                    .GetHistoricalCountAsync(DataPointType.Trade, Exchange.Binance, "BTC", "USDT", start, end))
                .Result;

            Output.WriteLine($"{count.Count}");
            count.Count.Should().BeGreaterThan(0);
        }
    }
}
