using System;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Options;
using Serilog;

namespace Trakx.Shrimpy.ApiClient.Utils
{
    public class ApiKeyCredentialsProvider : ICredentialsProvider, IDisposable
    {
        internal const string ApiKeyHeader = "DEV-SHRIMPY-API-KEY";
        internal const string ApiNonceHeader = "DEV-SHRIMPY-API-NONCE";
        internal const string ApiSignatureHeader = "DEV-SHRIMPY-API-SIGNATURE";

        private readonly ShrimpyApiConfiguration _configuration;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly CancellationTokenSource _tokenSource;
        private readonly byte[] _encodingSecret;

        private static readonly ILogger Logger = Log.Logger.ForContext<MarketDataClient>();

        public ApiKeyCredentialsProvider(IOptions<ShrimpyApiConfiguration> configuration, 
            IDateTimeProvider dateTimeProvider)
        {
            _configuration = configuration.Value;
            _dateTimeProvider = dateTimeProvider;

            _tokenSource = new CancellationTokenSource();
            _encodingSecret = Convert.FromBase64String(_configuration.ApiSecret);
        }

        
        #region Implementation of ICredentialsProvider

        /// <inheritdoc />
        public void AddCredentials(HttpRequestMessage msg)
        {
            var path = msg.RequestUri.AbsolutePath;
            var method = msg.Method.Method.ToUpperInvariant();
            var nonce = GetNonce();
            var body = msg.Content.ReadAsStringAsync();

            var prehashString = path + method + nonce + body;
            Logger.Verbose("PreHash string is {prehashString}", prehashString);

            msg.Headers.Add(ApiKeyHeader, _configuration.ApiKey);
            
            msg.Headers.Add(ApiNonceHeader, nonce);
            msg.Headers.Add(ApiSignatureHeader, GetSignature(prehashString));
            Logger.Verbose("Headers added");
        }
        #endregion

        private string GetNonce() => _dateTimeProvider.UtcNowAsOffset.ToUnixTimeMilliseconds()
            .ToString(CultureInfo.InvariantCulture);
        private string GetSignature(string preHash) => Convert.ToBase64String(new HMACSHA256(_encodingSecret)
            .ComputeHash(Encoding.UTF8.GetBytes(preHash)));
        
        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            _tokenSource.Cancel();
            _tokenSource?.Dispose();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}