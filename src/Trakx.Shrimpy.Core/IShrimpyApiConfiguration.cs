using System.Collections.Generic;

namespace Trakx.Shrimpy.Core
{
    public interface IShrimpyApiConfiguration
    {
        string BaseUrl { get; init; }
        string ApiKey { get; init; }
        string ApiSecret { get; init; }
        List<string> FavouriteExchanges { get; init; }
    }
}
