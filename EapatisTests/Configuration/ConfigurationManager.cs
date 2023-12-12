using Microsoft.Extensions.Configuration;

namespace EapatisTests.Configuration;

public class ConfigurationManager
{
    public static IConfiguration TestSettings { get; }

    static ConfigurationManager()
    {
        try
        {
            TestSettings = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("eapatis-tests-settings.json")
                .Build();
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
            throw;
        }
    }
}