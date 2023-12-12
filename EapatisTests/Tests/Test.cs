using EapatisTests.Common.Extension;
using EapatisTests.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace EapatisTests.Tests;

public class WorkCheckingTests(ITestOutputHelper output)
: FixtureBase(collection => collection.Register()
    .AddSingleton<TestContainersProvider>(),
    output
)
{
    [Fact]
    public async Task Test()
    {
        var testContainersProvider = Resolve<TestContainersProvider>();
        await testContainersProvider.InitializeAsync();
        var remoteWebDriver = testContainersProvider.CreateRemoteWebDriver();
    }

    [Fact]
    public void NotValid()
    {
        Assert.Equal("qwe", "123");
    }

    [Fact]
    public void Valid()
    {
        Assert.Equal(1, 1);
    }
}