using Microsoft.Extensions.DependencyInjection;

namespace EapatisTests.Builders;

public static class ServiceProviderBuilder
{
    public static ServiceProvider Build(Action<IServiceCollection> configureServices)
    {
        ServiceCollection serviceCollections = new();
        configureServices(serviceCollections);
        return serviceCollections.BuildServiceProvider();
    }
}