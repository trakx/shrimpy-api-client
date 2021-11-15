using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Serilog;
using Trakx.Shrimpy.Core;

namespace Trakx.Shrimpy.DeveloperApiClient
{
    public static partial class AddShrimpyDeveloperClientExtensions
    {
        public static void AddClients(this IServiceCollection services)
        {
            var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromMilliseconds(100), retryCount: 5);
            
            services.AddHttpClient<IHistoricalClient, HistoricalClient>("Trakx.Shrimpy.DeveloperApiClient.HistoricalClient")
                .AddPolicyHandler((s, request) =>
                    Policy<HttpResponseMessage>
                    .Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = Log.Logger.ForContext<HistoricalClient>();
                            logger.LogFailure(result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("Trakx.Shrimpy.DeveloperApiClient.HistoricalClient"));

                    }
    }
}
