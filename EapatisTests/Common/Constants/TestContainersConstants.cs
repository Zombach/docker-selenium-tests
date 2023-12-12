namespace EapatisTests.Common.Constants;

public static class TestContainersConstants
{
    public static string GetUniqueId => Guid.NewGuid().ToString("D");

    public const string SeleniumHub = "selenium-hub";
    public const string SeleniumHubDependency = "selenium/hub";
    public const string SeleniumChrome = "selenium-chrome";
    public const string SeleniumChromeDependency = "selenium/node-chrome";

    public const int Port4442 = 4442;
    public const int Port4443 = 4443;
    public const int Port4444 = 4444;

    public const string SeEventBusHost = "SE_EVENT_BUS_HOST";
    public const string SeEventBusSubscribePort = "SE_EVENT_BUS_SUBSCRIBE_PORT";
    public const string SeEventBusPublishPort = "SE_EVENT_BUS_PUBLISH_PORT";
}