namespace MusicManager.Core.Processors;

using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using MusicManager.Core.Model;

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

    public void Process(Artist artist, Album album, Track track)
    {
        ArgumentNullException.ThrowIfNull(artist);
        ArgumentNullException.ThrowIfNull(album);
        ArgumentNullException.ThrowIfNull(track);

        IMusicFile? musicFile = this.musicFileFactory.Load(track.Path);
        if (musicFile == null)
        {
            this.logger.LogWarning("File not supported: {TrackPath}", track.Path);
            return;
        }

        this.logger.LogInformation("Processing tags: {TrackName}", track.Name);

        musicFile.AlbumArtist = artist.Name;

        if (!string.IsNullOrEmpty(musicFile.Genre)
            && musicFile.Genre.Contains("unknown", StringComparison.CurrentCultureIgnoreCase))
        {
            musicFile.Genre = null;
        }

        musicFile.Comment = null;

        musicFile.Save();
    }
}
