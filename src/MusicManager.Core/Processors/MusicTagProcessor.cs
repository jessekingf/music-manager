namespace MusicManager.Core.Processors;

using System.IO.Abstractions;
using Microsoft.Extensions.Logging;

public class MusicTagProcessor : IMusicFileProcessor
{
    private readonly ILogger<MusicProcessor> logger;
    private readonly IFileSystem fileSystem;
    private readonly IMusicFileFactory musicFileFactory;

    public MusicTagProcessor(ILogger<MusicProcessor> logger, IFileSystem fileSystem, IMusicFileFactory musicFileFactory)
    {
        this.logger = logger;
        this.fileSystem = fileSystem;
        this.musicFileFactory = musicFileFactory;
    }

    public string Process(string artistName, string albumName, string filePath)
    {
        IMusicFile? musicFile = this.LoadFile(filePath);
        if (musicFile == null)
        {
            return filePath;
        }

        this.logger.LogInformation("Processing tags: {FilePath}", filePath);

        musicFile.AlbumArtist = artistName;
        musicFile.Comment = null;

        if (!string.IsNullOrEmpty(musicFile.Genre)
            && musicFile.Genre.Contains("unknown", StringComparison.CurrentCultureIgnoreCase))
        {
            musicFile.Genre = null;
        }

        musicFile.Save();

        return filePath;
    }

    private IMusicFile? LoadFile(string filePath)
    {
        try
        {
            return this.musicFileFactory.Load(filePath);
        }
        catch (InvalidOperationException)
        {
            this.logger.LogWarning("File not supported: {FilePath}", filePath);
            return null;
        }
    }
}
