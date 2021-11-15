using System.Collections.Generic;
using Trakx.Shrimpy.Core;
using Trakx.Utils.Attributes;

namespace Trakx.Shrimpy.DeveloperApiClient
{
    public record ShrimpyDevApiConfiguration : IShrimpyApiConfiguration
    {
#nullable disable
        public string BaseUrl { get; init; }

        [SecretEnvironmentVariable]
        public string ApiKey { get; init; }

        [SecretEnvironmentVariable]
        public string ApiSecret { get; init; }
        public List<string> FavouriteExchanges { get; init; }
#nullable restore
    }
}
