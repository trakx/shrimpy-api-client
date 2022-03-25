using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Trakx.Utils.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.Shrimpy.ApiClient.Tests.Integration;

[Collection(nameof(ApiTestCollection))]
public class ShrimpyClientTestsBase
{
    protected readonly ITestOutputHelper Output;
    protected readonly ILogger Logger;
    protected readonly IServiceProvider ServiceProvider;

    public ShrimpyClientTestsBase(ShrimpyApiFixture apiFixture, ITestOutputHelper output)
    {
        Output = output;
        ServiceProvider = apiFixture.ServiceProvider;
        Logger = new LoggerConfiguration().WriteTo.TestOutput(output).CreateLogger();
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

        var configuration = ConfigurationHelper.GetConfigurationFromEnv<ShrimpyApiConfiguration>()
            with {
                BaseUrl = "https://api.shrimpy.io"
            };

        var serviceCollection = new ServiceCollection();

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
