using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.Shrimpy.ApiClient.Tests.Integration
{
    public class MarketDataClientTests : ShrimpyClientTestsBase
    {
        private readonly IMarketDataClient _marketDataClient;

        public MarketDataClientTests(ITestOutputHelper output) : base(output)
        {
            _marketDataClient = ServiceProvider.GetRequiredService<IMarketDataClient>();
        }

        [Fact]
        public async Task GetTicker_should_return_all_tickers()
        {
            var tickers = await _marketDataClient.GetTickerAsync("binance");
            tickers.Result.Count.Should().BeGreaterThan(10);
            var knownSymbols = tickers.Result.Select(t => t.Symbol);
            _logger.Information(string.Join(",", knownSymbols));
        }
    }
}
