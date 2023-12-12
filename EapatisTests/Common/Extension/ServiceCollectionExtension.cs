using EapatisTests.Builders;
using EapatisTests.Configuration;
using EapatisTests.Providers;
using EapatisTests.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EapatisTests.Common.Extension;

public static class ServiceCollectionExtension
{
    public static IServiceCollection Register(this IServiceCollection collection)
    {
        collection.AddOptionsService(ConfigurationManager.TestSettings);
        collection.AddSingleton<TestContainersProviderBuilder>();
        collection.AddScoped<WebClientProvider>();
        return collection;
    }
}