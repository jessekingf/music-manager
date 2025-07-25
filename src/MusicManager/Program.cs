namespace MusicManager;

using Microsoft.Extensions.Hosting;
using MusicManager.Commands;
using MusicManager.Exceptions;

internal class Program
{
    public static void Main(string[] args)
    {
        using IHost host = Startup.CreateHost(args);

        try
        {
            CommandParser commandParser = new(host);
            ICommand command = commandParser.Parse(args);

            command.Execute();
        }
        catch (InvalidOptionException ex)
        {
            Console.Error.WriteLine(ex.Message);
            new HelpCommand().Execute();

            Environment.ExitCode = 1;
            return;
        }
    }
}
