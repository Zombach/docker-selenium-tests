using System.Collections.Concurrent;
using EapatisTests.Builders;
using EapatisTests.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace EapatisTests;

public abstract class FixtureBase
(
    Action<IServiceCollection> configureServices,
    ITestOutputHelper output
) : XunitContextBase(output)
{
    private string CurrentTestId => Context.Test.TestCase.UniqueID;

    private readonly ServiceProvider _serviceProvider = ServiceProviderBuilder.Build(configureServices);
    private readonly ConcurrentDictionary<string, AsyncServiceScope> _serviceScopes = new();

    protected TService Resolve<TService>() where TService : notnull
    => _serviceScopes.GetOrAdd(CurrentTestId, _ => _serviceProvider.CreateAsyncScope())
        .ServiceProvider.GetRequiredService<TService>();

    protected async Task<WebClientProvider> GetWebClientProvider()
    {
        var testContainersProvider = Resolve<TestContainersProvider>();
        await testContainersProvider.InitializeAsync();
        var remoteWebDriver = testContainersProvider.CreateRemoteWebDriver();
        var webClientProvider = Resolve<WebClientProvider>();
        webClientProvider.Initial(remoteWebDriver);
        return webClientProvider;
    }

    public override void Dispose()
    {
        Task.Run(async () => await DisposeAsync()).ConfigureAwait(false);
        base.Dispose();
    }

    private async ValueTask DisposeAsync()
    => await _serviceProvider.DisposeAsync();
}