namespace MusicCleaner;

using System.IO.Abstractions;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MusicCleaner.Commands;
using MusicCleaner.Core;
using MusicCleaner.Core.Processors;

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

    private static Stream LoadAppSettings()
    {
        Assembly? assembly = Assembly.GetEntryAssembly();
        Stream? appSettingsSteam = assembly?.GetManifestResourceStream("MusicCleaner.appsettings.json");

        if (appSettingsSteam == null)
        {
            throw new InvalidOperationException("No application settings file found in embedded resources.");
        }

        return appSettingsSteam;
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        AddCommands(services);
        AddMusicProcessor(services);

        services.AddTransient<IFileSystem, FileSystem>();
    }

    private static void AddCommands(IServiceCollection services)
    {
        services.AddTransient<CleanMusicCommand>();
        services.AddTransient<HelpCommand>();
        services.AddTransient<VersionCommand>();
    }

    private static void AddMusicProcessor(IServiceCollection services)
    {
        AddMusicProcessors(services);

        services.AddTransient<MusicProcessor>();
        services.AddTransient<IMusicFileFactory, MusicFileFactory>();
    }

    private static void AddMusicProcessors(IServiceCollection services)
    {
        services.AddTransient<IMusicFileProcessor, MusicTagProcessor>();
    }
}
