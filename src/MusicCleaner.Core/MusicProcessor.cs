namespace MusicCleaner.Core;

using System.IO.Abstractions;
using Microsoft.Extensions.Logging;

public class MusicProcessor
{
    private readonly ILogger<MusicProcessor> logger;
    private readonly IFileSystem fileSystem;

    public MusicProcessor(ILogger<MusicProcessor> logger, IFileSystem fileSystem)
    {
        this.logger = logger;
        this.fileSystem = fileSystem;
    }

    public void Clean(string musicPath)
    {
        this.logger.LogInformation("Processing music directory: {MusicDir}", musicPath);

        if (!this.fileSystem.Directory.Exists(musicPath))
        {
            this.logger.LogWarning("The music directory does not exist: {MusicDir}", musicPath);
            return;
        }

        // TODO: Clean music.
    }
}
