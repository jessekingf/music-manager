namespace MusicManager.Commands;

internal class HelpCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine(Localization.Get("ProgramHelp"));
    }
}
