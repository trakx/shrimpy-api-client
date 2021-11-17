using Xunit.Abstractions;

namespace Trakx.Shrimpy.Core.Tests.Integration
{
    public class EnvFileDocumentationUpdater : Trakx.Utils.Testing.EnvFileDocumentationUpdaterBase
    {
        public EnvFileDocumentationUpdater(ITestOutputHelper output) : base(output)
        {
        }
    }
}
