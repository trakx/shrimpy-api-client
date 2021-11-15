using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.Shrimpy.ApiClient.Tests.Integration
{
    public class HistoricalClientTests : ShrimpyClientTestsBase
    {
        private readonly IHistoricalClient _historicalClient;

        public HistoricalClientTests(ShrimpyApiFixture apiFixture, ITestOutputHelper output) : base(apiFixture, output)
        {
            _historicalClient = ServiceProvider.GetRequiredService<IHistoricalClient>();
        }

        [Fact]
        public async Task GetHistoricalCandlesAsync_should_return_tickers()
        {
            var tickers = await _historicalClient.GetHistoricalCandlesAsync(Exchange.Binance,
                "btc",
                "usdc",
                DateTimeOffset.Now,
                DateTimeOffset.Now.AddMinutes(2),
                10,
                "1m");
            tickers.Should().NotBeNull();
        }
    }
}
