using Xunit.Abstractions;

namespace Trakx.Shrimpy.Core.Tests.Integration
{
    public class OpenApiGeneratedCodeModifier : Trakx.Utils.Testing.OpenApiGeneratedCodeModifier
    {
        public OpenApiGeneratedCodeModifier(ITestOutputHelper output) : base(output)
        {
        }
    }
}
