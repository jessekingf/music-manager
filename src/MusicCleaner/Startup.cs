namespace MusicCleaner;

using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

internal static class Startup
{
    public static IHost CreateHost(string[] args)
    {
        using Stream appSettingsSteam = LoadAppSettings();

        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonStream(appSettingsSteam);
            })
            .ConfigureLogging((context, logging) =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddConfiguration(context.Configuration.GetSection("Logging"));
            })
            .ConfigureServices((context, services) =>
            {
                ConfigureServices(services);
            })
            .Build();
    }

#pragma warning disable IDE0060 // Remove unused parameter
    private static void ConfigureServices(IServiceCollection services)
#pragma warning restore IDE0060 // Remove unused parameter
    {
    }

    private static Stream LoadAppSettings()
    {
        Assembly assembly = typeof(Program).Assembly;
        Stream? appSettingsSteam = assembly.GetManifestResourceStream("MusicCleaner.appsettings.json");

        if (appSettingsSteam == null)
        {
            throw new InvalidOperationException("No application settings file found in embedded resources.");
        }

        return appSettingsSteam;
    }
}
