using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Serilog;
using Trakx.Shrimpy.Core;
using Trakx.Shrimpy.Core.Utils;
using Trakx.Utils.DateTimeHelpers;

namespace Trakx.Shrimpy.Core
{
    public static partial class AddShrimpyCoreExtensions
    {
        public static IServiceCollection AddCoreDependencies(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<ShrimpyApiConfiguration>(
                configuration.GetSection(nameof(ShrimpyApiConfiguration)));
            AddCoreDependencies(services);

            return services;
        }

        public static IServiceCollection AddCoreDependencies(
            this IServiceCollection services, ShrimpyApiConfiguration apiConfiguration)
        {
            var options = Options.Create(apiConfiguration);
            services.AddSingleton(options);

            AddCoreDependencies(services);

            return services;
        }

        private static void AddCoreDependencies(IServiceCollection services)
        {
            services.AddSingleton<IShrimpyCredentialsProvider, ApiKeyCredentialsProvider>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        }
    }
}
