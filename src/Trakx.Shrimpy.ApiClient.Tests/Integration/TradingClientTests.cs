//using System;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using Xunit;
//using Xunit.Abstractions;

//namespace Trakx.Shrimpy.ApiClient.Tests.Integration
//{
//    public class TradingClientTests : ShrimpyClientTestsBase
//    {
//        private readonly ITradingClient _tradingClient;

//        public TradingClientTests(ITestOutputHelper output) : base(output)
//        {
//            _tradingClient = ServiceProvider.GetRequiredService<ITradingClient>();
//        }

//        [Fact]
//        public async Task GetBalance_should_retrieve_user_balance()
//        {
//            await _tradingClient.GetBalanceAsync(1232);
//        }
//    }
//}
