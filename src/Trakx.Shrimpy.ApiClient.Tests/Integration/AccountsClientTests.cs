using System.Text.Json;

namespace Trakx.Shrimpy.ApiClient.Tests.Integration;

public sealed class AccountsClientTests : ShrimpyClientTestsBase
{
    private readonly IAccountsClient _accountsClient;
    public AccountsClientTests(ShrimpyApiFixture apiFixture, ITestOutputHelper output) : base(apiFixture, output)
    {
        _accountsClient = ServiceProvider.GetRequiredService<IAccountsClient>();
    }

    [Fact]
    public async Task ListAccounts_and_GetAccount_should_return_results()
    {
        var accounts = (await _accountsClient.ListAccountsAsync()).Content;
        var first = accounts.First();

        Logger.Information("Found accounts: {accounts}",
            JsonSerializer.Serialize(accounts));

        var account = (await _accountsClient.GetAccountAsync(first.Id)).Content;

        account.Id.Should().Be(first.Id);
        account.Exchange.Should().Be(first.Exchange);
        account.IsRebalancing.Should().Be(first.IsRebalancing);
    }

    [Fact]
    public async Task GetBalance_should_work()
    {
        var accounts = (await _accountsClient.ListAccountsAsync()).Content;
        var binanceAccountId = accounts.Single(a => a.Exchange == Exchange.Binance).Id;

        var balances = (await _accountsClient.GetBalanceAsync(binanceAccountId)).Content.Balances;
        balances.Count.Should().BeGreaterThan(1);
        Logger.Information("Binance balances: {balances}",
            string.Join(", ", JsonSerializer.Serialize(balances)));

        balances.First(b => string.Equals(b.Symbol, "eur", StringComparison.InvariantCultureIgnoreCase)).UsdValue
            .Should().NotBe(0);
    }
}
