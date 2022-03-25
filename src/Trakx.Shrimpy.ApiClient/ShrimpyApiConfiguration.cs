using System.Collections.Generic;
using Trakx.Utils.Attributes;

namespace Trakx.Shrimpy.ApiClient
{
    public record ShrimpyApiConfiguration
    {
#nullable disable
        public string BaseUrl { get; init; }

        [AwsParameter]
        [SecretEnvironmentVariable]
        public string ApiKey { get; init; }

        [AwsParameter]
        [SecretEnvironmentVariable]
        public string ApiSecret { get; init; }
        public List<string> FavouriteExchanges { get; init; }
#nullable restore
    }
}
