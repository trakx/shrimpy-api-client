using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Serilog;
using Trakx.Common.DateAndTime;

namespace Trakx.Shrimpy.ApiClient.Utils;

public interface IShrimpyCredentialsProvider : ICredentialsProvider { }
public class ApiKeyCredentialsProvider : IShrimpyCredentialsProvider, IDisposable
{
    private const string ApiKeyHeader = "SHRIMPY-API-KEY";
    private const string ApiNonceHeader = "SHRIMPY-API-NONCE";
    private const string ApiSignatureHeader = "SHRIMPY-API-SIGNATURE";

    private readonly ShrimpyApiConfiguration _configuration;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly CancellationTokenSource _tokenSource;
    private readonly byte[] _encodingSecret;

    private static readonly ILogger Logger = Log.Logger.ForContext<ApiKeyCredentialsProvider>();

    public ApiKeyCredentialsProvider(ShrimpyApiConfiguration configuration,
        IDateTimeProvider dateTimeProvider)
    {
        _configuration = configuration;
        _dateTimeProvider = dateTimeProvider;

        _tokenSource = new CancellationTokenSource();
        _encodingSecret = Convert.FromBase64String(_configuration.ApiSecret);
    }


    #region Implementation of ICredentialsProvider

    /// <inheritdoc />
    public void AddCredentials(HttpRequestMessage msg)
    {
        var path = msg.RequestUri!.AbsolutePath;
        var method = msg.Method.Method.ToUpperInvariant();
        var nonce = GetNonce();
        var body = msg.Content?.ReadAsStringAsync().GetAwaiter().GetResult() ?? string.Empty;

        var prehashString = path + method + nonce + body;
        Logger.Verbose("PreHash string is {prehashString}", prehashString);

        var devPrefix = msg.RequestUri.Host.Contains("dev-api") ? "DEV-" : string.Empty;

        msg.Headers.Add(devPrefix + ApiKeyHeader, _configuration.ApiKey);
        msg.Headers.Add(devPrefix + ApiNonceHeader, nonce);
        msg.Headers.Add(devPrefix + ApiSignatureHeader, GetSignature(prehashString));
        Logger.Verbose("Headers added");
    }

    public Task AddCredentialsAsync(HttpRequestMessage msg)
    {
        AddCredentials(msg);
        return Task.CompletedTask;
    }
    #endregion

    private string GetNonce() => _dateTimeProvider.UtcNowAsOffset.Ticks
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