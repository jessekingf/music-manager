namespace MusicManager.Core;

using System.Collections.Generic;
using System.IO.Abstractions;
using Microsoft.Extensions.Logging;

public class MusicProcessor
{
    private readonly ILogger<MusicProcessor> logger;
    private readonly IFileSystem fileSystem;
    private readonly IEnumerable<IMusicFileProcessor> fileProcessors;

    public MusicProcessor(ILogger<MusicProcessor> logger, IFileSystem fileSystem, IEnumerable<IMusicFileProcessor> fileProcessors)
    {
        this.logger = logger;
        this.fileSystem = fileSystem;
        this.fileProcessors = fileProcessors;
    }

    public void Clean(string musicPath)
    {
        this.logger.LogInformation("Processing music directory: {MusicDir}", musicPath);

        if (!this.fileSystem.Directory.Exists(musicPath))
        {
            this.logger.LogWarning("The music directory does not exist: {MusicDir}", musicPath);
            return;
        }

        string[] artistDirs = this.fileSystem.Directory.GetDirectories(musicPath);
        foreach (string artistPath in artistDirs)
        {
            this.ProcessArtist(artistPath);
        }
    }

    private void ProcessArtist(string artistPath)
    {
        string? artistName = this.fileSystem.Path.GetFileName(artistPath);
        if (string.IsNullOrWhiteSpace(artistName))
        {
            return;
        }

        this.logger.LogInformation("Processing artist {ArtistName}: {ArtistPath}", artistName, artistPath);

        string[] albumDirs = this.fileSystem.Directory.GetDirectories(artistPath);
        foreach (string albumPath in albumDirs)
        {
            this.ProcessAlbum(artistName, albumPath);
        }
    }

    private void ProcessAlbum(string artistName, string albumPath)
    {
        string? albumName = this.fileSystem.Path.GetFileName(albumPath);
        if (string.IsNullOrWhiteSpace(albumName))
        {
            return;
        }

        this.logger.LogInformation("Processing album {ArtistName} - {AlbumName}: {AlbumPath}", artistName, albumName, albumPath);

        string[] musicFiles = this.fileSystem.Directory.GetFiles(albumPath);

        foreach (string trackPath in musicFiles)
        {
            this.ProcessFile(artistName, albumName, trackPath);
        }
    }

    private void ProcessFile(string artistName, string albumName, string filePath)
    {
        string fileName = this.fileSystem.Path.GetFileName(filePath);

        this.logger.LogInformation("Processing track {ArtistName} - {AlbumName} - {TrackName} : {AlbumPath}", artistName, albumName, fileName, filePath);

        foreach (IMusicFileProcessor fileProcessor in this.fileProcessors)
        {
            filePath = fileProcessor.Process(artistName, albumName, filePath);
        }
    }
}
