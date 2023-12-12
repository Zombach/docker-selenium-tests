using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using EapatisTests.Common.Constants;
using EapatisTests.Configuration;

namespace EapatisTests.Builders;

public class TestContainersProviderBuilder(TestContainersOptions testContainersOptions)
{
    public INetwork CreateNetworkContainer() => new NetworkBuilder()
        .WithName(TestContainersConstants.GetUniqueId)
        .Build();

    public IContainer CreateSeleniumHub(INetwork network) => new ContainerBuilder()
        .WithHostname(TestContainersConstants.GetUniqueId)
        .WithName($"{TestContainersConstants.SeleniumHub}-{TestContainersConstants.GetUniqueId}")
        .WithImage($"{TestContainersConstants.SeleniumHubDependency}:{testContainersOptions.SeleniumHubVersion}")
        .WithPortBinding(TestContainersConstants.Port4442, true)
        .WithPortBinding(TestContainersConstants.Port4443, true)
        .WithPortBinding(TestContainersConstants.Port4444, true)
        .WithNetwork(network)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(TestContainersConstants.Port4444))
        .Build();

    public IContainer CreateSeleniumChrome(INetwork network, string seEventBusHost) => new ContainerBuilder()
        .WithName($"{TestContainersConstants.SeleniumChrome}-{TestContainersConstants.GetUniqueId}")
        .WithImage($"{TestContainersConstants.SeleniumChromeDependency}:{testContainersOptions.SeleniumChromeVersion}")
        .WithEnvironment(TestContainersConstants.SeEventBusHost, seEventBusHost)
        .WithEnvironment(TestContainersConstants.SeEventBusSubscribePort, TestContainersConstants.Port4443.ToString())
        .WithEnvironment(TestContainersConstants.SeEventBusPublishPort, TestContainersConstants.Port4444.ToString())
        .WithCreateParameterModifier
        (
            modifier =>
                modifier.HostConfig.ShmSize = testContainersOptions.SeleniumChromeShmSize
        )
        .WithNetwork(network)
        .Build();
}