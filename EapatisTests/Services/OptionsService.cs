using EapatisTests.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EapatisTests.Services;

public static class OptionsService
{
    public static IServiceCollection AddOptionsService(this IServiceCollection services, IConfiguration configuration)
    {
        var eapatisOptions = configuration.GetOptions<EapatisOptions>(EapatisOptions.SectionKey);
        ArgumentNullException.ThrowIfNull(eapatisOptions, $"Не удалось получить настройки для: {EapatisOptions.SectionKey}");
        services.AddSingleton(eapatisOptions);

        var testContainersOptions = configuration.GetOptions<TestContainersOptions>(TestContainersOptions.SectionKey);
        ArgumentNullException.ThrowIfNull(testContainersOptions, $"Не удалось получить настройки для: {TestContainersOptions.SectionKey}");
        services.AddSingleton(testContainersOptions);

        return services;
    }

    private static TOptions? GetOptions<TOptions>(this IConfiguration configuration, string sectionsKey)
    => configuration.GetSection(sectionsKey).Get<TOptions>();
}