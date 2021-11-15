using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Trakx.Shrimpy.ApiClient;
using Trakx.Shrimpy.DeveloperApiClient;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.AssemblyFixture;

namespace Trakx.Shrimpy.Core.Tests.Integration
{
    public class ShrimpyClientTestsBase : IAssemblyFixture<ShrimpyApiFixture>
    {
        protected ServiceProvider ServiceProvider;
        protected ILogger Logger;

        public ShrimpyClientTestsBase(ShrimpyApiFixture apiFixture, ITestOutputHelper output)
        {
            Logger = new LoggerConfiguration().WriteTo.TestOutput(output).CreateLogger();

            ServiceProvider = apiFixture.ServiceProvider;
        }
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

            var devConfig = new ShrimpyDevApiConfiguration
            {
                ApiKey = secrets.ShrimpyApiKey,
                ApiSecret = secrets.ShrimpyApiSecret,
                BaseUrl = "https://dev-api.shrimpy.io"
            };

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddCoreDependencies();
            serviceCollection.AddApiCredentialsProvider<ShrimpyDevApiConfiguration>();
            serviceCollection.AddApiCredentialsProvider<ShrimpyApiConfiguration>();


            serviceCollection.AddShrimpyClients(configuration);
            serviceCollection.AddDeveloperClients(devConfig);
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
