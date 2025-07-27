namespace MusicManager;

using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicManager.Commands;
using MusicManager.Exceptions;

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
                switch (arg.ToLower(CultureInfo.CurrentCulture))
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

        ProcessMusicCommand cleanMusicCommand = this.host.Services.GetRequiredService<ProcessMusicCommand>();
        cleanMusicCommand.MusicPath = Environment.ExpandEnvironmentVariables(nonSwitchArgs[0]);

        return cleanMusicCommand;
    }
}
