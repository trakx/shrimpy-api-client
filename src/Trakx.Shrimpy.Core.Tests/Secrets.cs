using Trakx.Shrimpy.ApiClient;
using Trakx.Utils.Attributes;
using Trakx.Utils.Testing;

namespace Trakx.Shrimpy.Core.Tests
{
    public record Secrets : SecretsBase
    {
        [SecretEnvironmentVariable(nameof(ShrimpyApiConfiguration), nameof(ShrimpyApiConfiguration.ApiKey))]
        public string? ShrimpyApiKey { get; init; }
        [SecretEnvironmentVariable(nameof(ShrimpyApiConfiguration), nameof(ShrimpyApiConfiguration.ApiSecret))]
        public string? ShrimpyApiSecret { get; init; }
    }

}
