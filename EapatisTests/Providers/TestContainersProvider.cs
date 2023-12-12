using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using EapatisTests.Builders;
using EapatisTests.Common.Constants;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace EapatisTests.Providers;

public class TestContainersProvider(TestContainersProviderBuilder testContainersProviderBuilder) : IAsyncDisposable
{
    private INetwork? _network;
    private IContainer? _seleniumHub;
    private IContainer? _seleniumChrome;

    public async Task InitializeAsync()
    {
        if (_network is null)
        {
            _network = testContainersProviderBuilder.CreateNetworkContainer();
            await _network.CreateAsync().ConfigureAwait(false);
        }

        if (_seleniumHub is null)
        {
            _seleniumHub = testContainersProviderBuilder.CreateSeleniumHub(_network);
            await _seleniumHub.StartAsync().ConfigureAwait(false);
        }

        if (_seleniumChrome is null)
        {
            _seleniumChrome = testContainersProviderBuilder.CreateSeleniumChrome(_network, _seleniumHub.Name[1..]);
            await _seleniumChrome.StartAsync().ConfigureAwait(false);
        }

        using var client = new HttpClient();
        client.BaseAddress = GetUriSeleniumHub();
        var repeat = 15;
        while (repeat > 0)
        {
            try
            {
                var answer = await client.GetStringAsync("/wd/hub/status");
                if (answer.Contains("\"ready\": true")) { break; }
                repeat--;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
                repeat -= 5;
            }
            await Task.Delay(1000);
        }
    }

    public async Task RestartChromeContainerAsync()
    {
        ArgumentNullException.ThrowIfNull(_seleniumChrome, $"Не инициализирован, {_seleniumChrome}");
        await StartChromeContainerAsync(_seleniumChrome);
        await StopChromeContainerAsync(_seleniumChrome);
    }

    public RemoteWebDriver CreateRemoteWebDriver()
    {
        ArgumentNullException.ThrowIfNull(_seleniumHub, $"Не инициализирован, {_seleniumHub}");
        return new RemoteWebDriver
        (
            GetUriSeleniumHub(),
            new ChromeOptions().ToCapabilities()
        );
    }
    
    private Uri GetUriSeleniumHub()
    {
        ArgumentNullException.ThrowIfNull(_seleniumHub, $"Не инициализирован, {_seleniumHub}");
        return new UriBuilder
        (
            Uri.UriSchemeHttp,
            _seleniumHub.Hostname,
            _seleniumHub.GetMappedPublicPort(TestContainersConstants.Port4444)
        ).Uri;
    }

    private async Task StartChromeContainerAsync(IContainer chromeContainer)
    => await chromeContainer.StartAsync().ConfigureAwait(false);

    private async Task StopChromeContainerAsync(IContainer chromeContainer)
    => await chromeContainer.StopAsync().ConfigureAwait(false);

    public async ValueTask DisposeAsync()
    {
        if (_seleniumChrome is not null)
        {
            await _seleniumChrome.StopAsync().ConfigureAwait(false);
            await _seleniumChrome.DisposeAsync().ConfigureAwait(false);
        }

        if (_seleniumHub is not null)
        {
            await _seleniumHub.StopAsync().ConfigureAwait(false);
            await _seleniumHub.DisposeAsync().ConfigureAwait(false);
        }

        if (_network is not null)
        {
            await _network.DeleteAsync().ConfigureAwait(false);
            await _network.DisposeAsync().ConfigureAwait(false);
        }
    }
}