using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
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

    }
}
