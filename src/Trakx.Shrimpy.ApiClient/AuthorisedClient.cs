using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Trakx.Shrimpy.ApiClient.Utils;

namespace Trakx.Shrimpy.ApiClient
{
    internal abstract class AuthorisedClient : FavouriteExchangesClient
    {
        private string ApiKey => ApiConfiguration!.ApiKey;

        protected AuthorisedClient(ClientConfigurator clientConfigurator) : base(clientConfigurator)
        {
            var credentialProvider = new ApiKeyCredentialsProvider();
        }

        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url);
        {
            AddA
        }

        
    }
}