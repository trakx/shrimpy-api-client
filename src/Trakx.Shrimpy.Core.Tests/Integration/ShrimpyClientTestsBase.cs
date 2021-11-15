using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Trakx.Shrimpy.ApiClient;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.Shrimpy.Core.Tests.Integration
{
    [Collection(nameof(ApiTestCollection))]
    public class ShrimpyClientTestsBase
    {
        protected ServiceProvider ServiceProvider;
        protected ILogger Logger;

        public ShrimpyClientTestsBase(ShrimpyApiFixture apiFixture, ITestOutputHelper output)
        {
            Logger = new LoggerConfiguration().WriteTo.TestOutput(output).CreateLogger();

            ServiceProvider = apiFixture.ServiceProvider;
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
        public ServiceProvider ServiceProvider { get; }

        public ShrimpyApiFixture()
        {
            var secrets = new Secrets();
            var configuration = new ShrimpyApiConfiguration
            {
                ApiKey = secrets.ShrimpyApiKey,
                ApiSecret = secrets.ShrimpyApiSecret,
                BaseUrl = "https://api.shrimpy.io"
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
