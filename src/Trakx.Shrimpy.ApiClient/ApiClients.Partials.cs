namespace Trakx.Shrimpy.ApiClient;

internal partial class AccountsClient
{
    partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
    {
        CredentialProvider.AddCredentials(request);
    }
}
