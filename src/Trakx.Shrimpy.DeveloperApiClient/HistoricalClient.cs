using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Trakx.Shrimpy.DeveloperApiClient
{
    public record HistoricalInstrumentKey
    {
#nullable disable
        public Exchange Exchange { get; init; }
        public string QuoteTradingSymbol { get; init; }
#nullable restore
    }

    public partial interface IHistoricalClient
    {
        System.Threading.Tasks.Task<Response<List<HistoricalCandle>>> GetHistoricalCandlesSafeAsync(Exchange exchange, string baseTradingSymbol, string quoteTradingSymbol, System.DateTimeOffset startTime, System.DateTimeOffset endTime, double limit, Interval interval, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
    }

    internal partial class HistoricalClient : IHistoricalClient
    {
        private ConcurrentDictionary<HistoricalInstrumentKey, List<HistoricalInstrument>?> cachedInstruments = new();
        public async System.Threading.Tasks.Task<Response<List<HistoricalCandle>>> GetHistoricalCandlesSafeAsync(Exchange exchange, string baseTradingSymbol, string quoteTradingSymbol, System.DateTimeOffset startTime, System.DateTimeOffset endTime, double limit, Interval interval, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var candleCacheRequest = new HistoricalInstrumentKey { Exchange = exchange, QuoteTradingSymbol = quoteTradingSymbol };

            if (!cachedInstruments.ContainsKey(candleCacheRequest))
            {
                var possibleSymbols = await GetHistoricalInstrumentsAsync(exchange, quoteTradingSymbol: quoteTradingSymbol);
                cachedInstruments.TryAdd(candleCacheRequest, possibleSymbols?.Result);
            }

            var containsKey = cachedInstruments.TryGetValue(candleCacheRequest, out var instruments);
            var hasInstrument = containsKey && (instruments?
                .Any(t => t.QuoteTradingSymbol.Equals(quoteTradingSymbol, StringComparison.InvariantCultureIgnoreCase)
                && t.BaseTradingSymbol.Equals(baseTradingSymbol, StringComparison.InvariantCultureIgnoreCase))
                ?? false);
            if (!hasInstrument)
            {
                return new Response<List<HistoricalCandle>>(500, new Dictionary<string, IEnumerable<string>>(), new());
            }
            return await GetHistoricalCandlesAsync(exchange, baseTradingSymbol, quoteTradingSymbol, startTime, endTime, limit, interval, cancellationToken);
        }
    }
}
