namespace MusicCleaner.Commands;

using System.Reflection;

internal class VersionCommand : ICommand
{
    public void Execute()
    {
        Version? version = Assembly.GetEntryAssembly()?.GetName()?.Version;
        if (version == null)
        {
            throw new InvalidOperationException("The application version was not found.");
        }

        Console.WriteLine(version.ToString());
    }
}
