namespace MusicManager.Commands;

using MusicManager.Core;

internal class ProcessMusicCommand : ICommand
{
    private readonly MusicProcessor musicProcessor;

    public ProcessMusicCommand(MusicProcessor musicProcessor)
    {
        this.musicProcessor = musicProcessor;
    }

    public string? MusicPath
    {
        get;
        set;
    }

    public void Execute()
    {
        if (string.IsNullOrWhiteSpace(this.MusicPath))
        {
            throw new InvalidOperationException("The music path was not set.");
        }

        this.musicProcessor.Process(this.MusicPath);
    }
}
