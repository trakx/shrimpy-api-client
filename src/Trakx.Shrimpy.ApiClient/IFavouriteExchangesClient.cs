﻿using System.Collections.Generic;

namespace Trakx.Shrimpy.ApiClient;

public interface IFavouriteExchangesClient
{
    IReadOnlyList<string> Top12ExchangeIds { get; }
    string Top12ExchangeIdsAsCsv { get; }
}