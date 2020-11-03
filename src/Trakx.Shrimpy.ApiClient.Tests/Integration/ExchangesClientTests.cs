using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.Shrimpy.ApiClient.Tests.Integration
{
    public class ExchangesClientTests : ShrimpyClientTestsBase
    {
        private readonly IExchangesClient _exchangesClient;
        
        public ExchangesClientTests(ITestOutputHelper output) : base(output)
        {
            _exchangesClient = ServiceProvider.GetRequiredService<IExchangesClient>();
        }

        [Fact]
        public async Task GetAllExchanges_should_return_all_available_exchanges()
        {
            var exchanges = (await _exchangesClient.ListExchangesAsync()).Result;
            exchanges.Count.Should().BeGreaterThan(10);

            var exchangesAsCsv = string.Join(",", exchanges.Select(e => e.Exchange).ToList());
            _logger.Information(exchangesAsCsv);
        }
    }
}
