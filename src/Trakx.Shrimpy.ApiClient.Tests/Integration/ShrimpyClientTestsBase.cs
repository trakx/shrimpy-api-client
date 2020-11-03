using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit.Abstractions;

namespace Trakx.Shrimpy.ApiClient.Tests.Integration
{
    public class ShrimpyClientTestsBase
    {
        protected ServiceProvider? ServiceProvider;
        protected ILogger _logger;

        public ShrimpyClientTestsBase(ITestOutputHelper output)
        {
            var configuration = new ShrimpyApiConfiguration
            {
                ApiKey = Secrets.ShrimpyApiKey,
                ApiSecret = Secrets.ShrimpyApiSecret
            };

            var serviceCollection = new ServiceCollection();

            _logger = new LoggerConfiguration().WriteTo.TestOutput(output).CreateLogger();

            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddSingleton(_logger);
            serviceCollection.AddShrimpyClient(configuration);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}