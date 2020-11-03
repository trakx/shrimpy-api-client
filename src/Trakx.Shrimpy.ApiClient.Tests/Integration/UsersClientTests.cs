using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.Shrimpy.ApiClient.Tests.Integration
{
    public class UsersClientTests : ShrimpyClientTestsBase
    {
        private readonly IUsersClient _usersClient;
        public UsersClientTests(ITestOutputHelper output) : base(output)
        {
            _usersClient = ServiceProvider.GetRequiredService<IUsersClient>();
        }

        [Fact]
        public async Task GetRecentVwapAsync_should_return_results()
        {
            var users = await _usersClient.ListUsersAsync();
            users.Result.Count.Should().BeGreaterOrEqualTo(1);
        }

    }
}
