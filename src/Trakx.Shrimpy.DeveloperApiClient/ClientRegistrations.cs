using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Serilog;
using Trakx.Shrimpy.Core;
using Trakx.Shrimpy.Core.Utils;

namespace Trakx.Shrimpy.DeveloperApiClient
{
    public static partial class AddShrimpyDeveloperClientExtensions
    {
        private static bool CanRetry(string errorMsg)
        {
            return !errorMsg.Contains("does not have data available.", StringComparison.InvariantCultureIgnoreCase);
        }

        private static void AddClients(this IServiceCollection services)
        {
            var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromMilliseconds(100), retryCount: 5);
            
            services.AddHttpClient<IHistoricalClient, HistoricalClient>("Trakx.Shrimpy.DeveloperApiClient.HistoricalClient")
                .AddPolicyHandler((s, request) =>
                    Policy<HttpResponseMessage>
                    .Handle<ApiException>(t => CanRetry(t.Message))
                    .Or<HttpRequestException>(t => CanRetry(t.Message))
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = Log.Logger.ForContext<HistoricalClient>();
                            logger.LogFailure(result, timeSpan, retryCount, context);

                            var credProvider = s.GetRequiredService<IShrimpyCredentialsProvider<ShrimpyDevApiConfiguration>>();
                            request.Headers.Clear();
                            credProvider.AddCredentials(request);
                        })
                    .WithPolicyKey("Trakx.Shrimpy.DeveloperApiClient.HistoricalClient"));

                    }
    }
}
