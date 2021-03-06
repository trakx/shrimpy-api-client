﻿using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Serilog;

namespace Trakx.Shrimpy.ApiClient
{
    public static partial class AddShrimpyClientExtension
    {
        private static void AddClients(this IServiceCollection services)
        {
            var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromMilliseconds(100), retryCount: 10, fastFirst: true);
                                    
            services.AddHttpClient<IMarketDataClient, MarketDataClient>("Trakx.Shrimpy.ApiClient.MarketDataClient")
                .AddPolicyHandler((s, request) => 
                    Policy<HttpResponseMessage>
                    .Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = Log.Logger.ForContext<MarketDataClient>();
                            LogFailure(logger, result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("Trakx.Shrimpy.ApiClient.MarketDataClient"));

                                
            services.AddHttpClient<IAccountsClient, AccountsClient>("Trakx.Shrimpy.ApiClient.AccountsClient")
                .AddPolicyHandler((s, request) => 
                    Policy<HttpResponseMessage>
                    .Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = Log.Logger.ForContext<AccountsClient>();
                            LogFailure(logger, result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("Trakx.Shrimpy.ApiClient.AccountsClient"));

        }
    }
}
