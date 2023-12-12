using System.Collections.Concurrent;
using EapatisTests.Builders;
using EapatisTests.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace EapatisTests;

public abstract class FixtureBase
(
    Action<IServiceCollection> configureServices,
    ITestOutputHelper output
) : XunitContextBase(output), IAsyncDisposable
{
    public string CurrentTestId => Context.Test.TestCase.UniqueID;

    private readonly ServiceProvider _serviceProvider = ServiceProviderBuilder.Build(configureServices);
    private readonly ConcurrentDictionary<string, AsyncServiceScope> _serviceScopes = new();

    protected TService Resolve<TService>() where TService : notnull
    => _serviceScopes.GetOrAdd(CurrentTestId, _ => _serviceProvider.CreateAsyncScope())
        .ServiceProvider.GetRequiredService<TService>();

    protected async Task<TestContainersProvider> GetTestContainersProvider()
    {
        var testContainersProvider = Resolve<TestContainersProvider>();
        await testContainersProvider.InitializeAsync();
        return testContainersProvider;
    }

    protected WebClientProvider GetWebClientProvider(TestContainersProvider testContainersProvider)
    {
        var remoteWebDriver = testContainersProvider.CreateRemoteWebDriver();
        var webClientProvider = Resolve<WebClientProvider>();
        webClientProvider.Initial(remoteWebDriver);
        return webClientProvider;
    }

    public void Dispose(string currentTestId)
    {
        if (_serviceScopes.ContainsKey(currentTestId) && _serviceScopes.Remove(currentTestId, out var asyncServiceScope))
        {
            Task.Run(async () => await asyncServiceScope.DisposeAsync()).ConfigureAwait(false);
        }
    }

    public async ValueTask DisposeAsync(string currentTestId)
    {
        if (_serviceScopes.ContainsKey(currentTestId) && _serviceScopes.Remove(currentTestId, out var asyncServiceScope))
        {
            await asyncServiceScope.DisposeAsync().ConfigureAwait(false);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _serviceProvider.DisposeAsync();
        base.Dispose();
    }
}