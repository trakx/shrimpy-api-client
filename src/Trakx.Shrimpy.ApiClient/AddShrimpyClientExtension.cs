﻿using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Serilog;
using Trakx.Shrimpy.ApiClient.Utils;
using Trakx.Utils.Apis;
using Trakx.Utils.DateTimeHelpers;

namespace Trakx.Shrimpy.ApiClient
{
    public static partial class AddShrimpyClientExtension
    {
        public static IServiceCollection AddShrimpyClient(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<ShrimpyApiConfiguration>(
                configuration.GetSection(nameof(ShrimpyApiConfiguration)));
            AddCommonDependencies(services);

            return services;
        }

        public static IServiceCollection AddShrimpyClient(
            this IServiceCollection services, ShrimpyApiConfiguration apiConfiguration)
        {
            var options = Options.Create(apiConfiguration);
            services.AddSingleton(options);
            
            AddCommonDependencies(services);

            return services;
        }

        private static void AddCommonDependencies(IServiceCollection services)
        {
            services.AddSingleton(s => new ClientConfigurator(s));
            services.AddSingleton<IShrimpyCredentialsProvider, ApiKeyCredentialsProvider>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            AddClients(services);
        }

        private static void LogFailure(ILogger logger, DelegateResult<HttpResponseMessage> result, TimeSpan timeSpan, int retryCount, Context context)
        {
            if (result.Exception != null)
            {
                logger.Warning(result.Exception, "An exception occurred on retry {RetryAttempt} for {PolicyKey}. Retrying in {SleepDuration}ms.",
                    retryCount, context.PolicyKey, timeSpan.TotalMilliseconds);
            }
            else
            {
                logger.Warning("A non success code {StatusCode} with reason {Reason} and content {Content} was received on retry {RetryAttempt} for {PolicyKey}. Retrying in {SleepDuration}ms.",
                    (int)result.Result.StatusCode, result.Result.ReasonPhrase,
                    result.Result.Content?.ReadAsStringAsync().GetAwaiter().GetResult(),
                    retryCount, context.PolicyKey, timeSpan.TotalMilliseconds);
            }
        }
    }
}