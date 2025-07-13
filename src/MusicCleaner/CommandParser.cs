namespace MusicCleaner;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicCleaner.Commands;
using MusicCleaner.Exceptions;

internal class CommandParser
{
    private readonly IHost host;

    public CommandParser(IHost host)
    {
        this.host = host;
    }

    public ICommand Parse(string[] args)
    {
        List<string> nonSwitchArgs = [];

        foreach (string arg in args ?? [])
        {
            if (string.IsNullOrEmpty(arg))
            {
                continue;
            }

            if (arg.StartsWith('-'))
            {
#pragma warning disable CA1308 // Normalize strings to uppercase
                switch (arg.ToLowerInvariant())
                {
                    case "--help":
                    case "-h":
                        return this.host.Services.GetRequiredService<HelpCommand>();
                    case "--version":
                    case "-v":
                        return this.host.Services.GetRequiredService<VersionCommand>();
                    default:
                        throw new InvalidOptionException(Localization.Get("ErrorInvalidOption", arg));
                }
#pragma warning restore CA1308 // Normalize strings to uppercase
            }

            nonSwitchArgs.Add(arg);
        }

        if (nonSwitchArgs.Count == 0)
        {
            return this.host.Services.GetRequiredService<HelpCommand>();
        }

        if (nonSwitchArgs.Count > 1)
        {
            throw new InvalidOptionException(Localization.Get("ErrorTooManyArgs"));
        }

        CleanMusicCommand cleanMusicCommand = this.host.Services.GetRequiredService<CleanMusicCommand>();
        cleanMusicCommand.MusicPath = Environment.ExpandEnvironmentVariables(nonSwitchArgs[0]);

        return cleanMusicCommand;
    }
}
