using System.Collections.Generic;

namespace Trakx.Shrimpy.Core
{
    public interface IFavouriteExchangesClient
    {
        IReadOnlyList<string> Top12ExchangeIds { get; }
        string Top12ExchangeIdsAsCsv { get; }
    }
}