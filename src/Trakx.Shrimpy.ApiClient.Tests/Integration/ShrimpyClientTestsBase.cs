using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.Shrimpy.ApiClient.Tests.Integration
{
    [Collection(nameof(ApiTestCollection))]
    public class ShrimpyClientTestsBase
    {
        protected ServiceProvider ServiceProvider;
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

    [CollectionDefinition(nameof(ApiTestCollection))]
    public class ApiTestCollection : ICollectionFixture<ShrimpyApiFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    public class ShrimpyApiFixture : IDisposable
    {
        public ServiceProvider ServiceProvider;

        public ShrimpyApiFixture()
        {
            var configuration = new ShrimpyApiConfiguration()
            {
                ApiKey = Secrets.ShrimpyApiKey,
                ApiSecret = Secrets.ShrimpyApiSecret,
                BaseUrl = "https://preprod.trakx.io/api/v2"
            };

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddShrimpyClient(configuration);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            ServiceProvider.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}