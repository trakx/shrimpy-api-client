using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        ///<summary>
        /// Method wraps around <see cref="GetHistoricalCandlesAsync"/> and validates the request against data recieved from <see cref="GetHistoricalInstrumentsAsync"/>
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <param name="exchange">The exchange for which to retrieve historical ohlcv data</param>
        /// <param name="baseTradingSymbol">The base trading symbol that is used by the exchange.</param>
        /// <param name="quoteTradingSymbol">The quote trading symbol that is used by the exchange.</param>
        /// <param name="startTime">The starting time in ISO 8601</param>
        /// <param name="endTime">The ending time in ISO 8601</param>
        /// <param name="limit">The amount of items to return. Must be an integer from 1 to 1000.</param>
        /// <param name="interval">The interval must be one of the following values 1m, 5m, 15m, 1h, 6h, or 1d) These values correspond to intervals representing one minute, five minutes, fifteen minutes, one hour, six hours, and one day, respectively.</param>
        /// <returns>The candles.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        Task<Response<List<HistoricalCandle>>> GetHistoricalCandlesSafeAsync(Exchange exchange, string baseTradingSymbol, string quoteTradingSymbol, DateTimeOffset startTime, DateTimeOffset endTime, double limit, Interval interval, CancellationToken cancellationToken = default(CancellationToken));
    }

    internal partial class HistoricalClient : IHistoricalClient
    {
        private readonly ConcurrentDictionary<HistoricalInstrumentKey, List<HistoricalInstrument>?> cachedInstruments = new();

        /// <inheritdoc />
        public async Task<Response<List<HistoricalCandle>>> GetHistoricalCandlesSafeAsync(Exchange exchange, string baseTradingSymbol, string quoteTradingSymbol, DateTimeOffset startTime, DateTimeOffset endTime, double limit, Interval interval, CancellationToken cancellationToken = default(CancellationToken))
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
