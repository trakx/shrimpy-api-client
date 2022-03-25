using Microsoft.Extensions.DependencyInjection;
using Trakx.Shrimpy.Core.Utils;
using Trakx.Utils.DateTimeHelpers;

namespace Trakx.Shrimpy.Core
{
    public static  class AddShrimpyCoreExtensions
    {
        public static void AddApiCredentialsProvider<TConfig>(this IServiceCollection services) where TConfig : class, IShrimpyApiConfiguration
        {
            services.AddSingleton<IShrimpyCredentialsProvider<TConfig>, ApiKeyCredentialsProvider<TConfig>>();
        }


    }
}
