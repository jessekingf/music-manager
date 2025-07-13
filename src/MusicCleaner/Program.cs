namespace MusicCleaner;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

internal class Program
{
    public static void Main(string[] args)
    {
        using IHost host = Startup.CreateHost(args);

        ILogger<Program> logger = host.Services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Music Cleaner");
    }
}
