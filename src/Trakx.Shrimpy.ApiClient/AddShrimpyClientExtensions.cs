using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Serilog;
using Trakx.Common.DateAndTime;
using Trakx.Shrimpy.ApiClient.Utils;

namespace Trakx.Shrimpy.ApiClient;

public static partial class AddShrimpyClientExtensions
{
    public static IServiceCollection AddShrimpyClient(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var config = configuration.GetSection(nameof(ShrimpyApiConfiguration)).Get<ShrimpyApiConfiguration>();
        serviceCollection.AddShrimpyClient(config);
        return serviceCollection;
    }

    public static IServiceCollection AddShrimpyClient(
        this IServiceCollection serviceCollection, ShrimpyApiConfiguration apiConfiguration)
    {
        serviceCollection.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        serviceCollection.AddSingleton<IShrimpyCredentialsProvider, ApiKeyCredentialsProvider>();
        serviceCollection.AddSingleton(apiConfiguration);
        serviceCollection.AddSingleton<ClientConfigurator>();

        serviceCollection.ConfigureHttpClient<IMarketDataClient, MarketDataClient>();
        serviceCollection.ConfigureHttpClient<IAccountsClient, AccountsClient>();

        return serviceCollection;
    }

    private static IServiceCollection ConfigureHttpClient<TClient,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
        this IServiceCollection services)
        where TClient : class
        where TImplementation : class, TClient
    {
        var name = typeof(TImplementation).FullName!;

        services
            .AddHttpClient<TClient, TImplementation>(name)
            .AddPolicyHandler<TImplementation>(name);

        return services;
    }

    private static IHttpClientBuilder AddPolicyHandler<TImplementation>(this IHttpClientBuilder http, string policyKey)
    {
        var medianFirstRetryDelay = TimeSpan.FromMilliseconds(100);
        var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay, retryCount: 10, fastFirst: true);

        return http.AddPolicyHandler((_, _) =>
            Policy<HttpResponseMessage>
            .Handle<ApiException>()
            .Or<HttpRequestException>()
            .OrTransientHttpStatusCode()
            .WaitAndRetryAsync(delay,
                onRetry: async (result, timeSpan, retryCount, context) =>
                {
                    var logger = Log.Logger.ForContext<TImplementation>();
                    await logger.LogApiFailure(result, timeSpan, retryCount, context);
                })
            .WithPolicyKey(policyKey)
        );
    }

    private static async Task LogApiFailure(this Serilog.ILogger logger,
        DelegateResult<HttpResponseMessage> result, TimeSpan timeSpan, int retryCount, Context context)
    {
        if (result.Exception != null)
        {
            logger.Warning(
                result.Exception,
                "An exception occurred on retry {RetryAttempt} for {PolicyKey} - Retrying in {SleepDuration}ms",
                retryCount, context.PolicyKey, timeSpan.TotalMilliseconds);
            return;
        }

        var message = result.Result;
        if (message == null) return;

        var content = await message.Content.ReadAsStringAsync();

        logger.Warning(
            "A non success code {StatusCode} with reason {Reason} and content {Content} was received on retry {RetryAttempt} for {PolicyKey} - Retrying in {SleepDuration}ms",
            (int)message.StatusCode, message.ReasonPhrase, content, retryCount, context.PolicyKey, timeSpan.TotalMilliseconds);
    }
}