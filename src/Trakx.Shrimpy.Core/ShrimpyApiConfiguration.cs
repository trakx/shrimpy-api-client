using System.Collections.Generic;
using Trakx.Utils.Attributes;

namespace Trakx.Shrimpy.Core
{
    public record ShrimpyApiConfiguration
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
