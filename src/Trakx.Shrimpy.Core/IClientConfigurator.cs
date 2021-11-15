using System;
using Trakx.Utils.Apis;

namespace Trakx.Shrimpy.Core
{
    public interface IClientConfigurator
    {
        ShrimpyApiConfiguration ApiConfiguration { get; }
        ICredentialsProvider GetCredentialProvider(Type clientType);
    }
}
