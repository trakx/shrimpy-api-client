
namespace Trakx.Shrimpy.DeveloperApiClient
{
    internal partial class HistoricalClient
    {
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
        {
            CredentialProvider.AddCredentials(request);
        }
    }
}
