using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit.Abstractions;

namespace Trakx.Shrimpy.ApiClient.Tests.Integration
{
    public class ShrimpyClientTestsBase
    {
        protected ServiceProvider? ServiceProvider;
        protected ILogger Logger;

        public ShrimpyClientTestsBase(ITestOutputHelper output)
        {
            var configuration = new ShrimpyApiConfiguration
            {
                BaseUrl = "https://api.shrimpy.io",
                ApiKey = Secrets.ShrimpyApiKey,
                ApiSecret = Secrets.ShrimpyApiSecret
            };

            var serviceCollection = new ServiceCollection();

            Logger = new LoggerConfiguration().WriteTo.TestOutput(output).CreateLogger();

            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddSingleton(Logger);
            serviceCollection.AddShrimpyClient(configuration);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}