using Trakx.Utils.Apis;

namespace Trakx.Shrimpy.Core
{
    public abstract class AuthorisedClient : FavouriteExchangesClient
    {
        protected readonly ICredentialsProvider CredentialProvider;
        protected string BaseUrl { get; }

        protected AuthorisedClient(IClientConfigurator clientConfigurator) : base(clientConfigurator)
        {
            CredentialProvider = clientConfigurator.GetCredentialProvider(GetType());
            BaseUrl = clientConfigurator.ApiConfiguration.BaseUrl;
        }
    }
}
