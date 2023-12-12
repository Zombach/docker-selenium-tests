namespace EapatisTests.Configuration;

public sealed record TestContainersOptions
(
    string SeleniumHubVersion,
    string SeleniumChromeVersion,
    long SeleniumChromeShmSize
)
{
    public const string SectionKey = nameof(TestContainersOptions);
}