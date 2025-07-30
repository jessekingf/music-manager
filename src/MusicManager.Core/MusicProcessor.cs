namespace MusicManager.Core;

using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using MusicManager.Core.Model;

public class MusicProcessor
{
    private static readonly string[] SupportedExtensions = [".mp3"];
    private readonly ILogger<MusicProcessor> logger;
    private readonly IFileSystem fileSystem;
    private readonly IEnumerable<IMusicFileProcessor> fileProcessors;
    private readonly Regex albumPattern = new(@"^\((?<year>\d{4})\)\s*(?<album>.+)$", RegexOptions.Compiled);
    private readonly Regex trackPattern = new(@"^(?<trackNumber>\d+)\s*(?:-\s*)?(?<trackName>.+)$", RegexOptions.Compiled);

    public MusicProcessor(
        ILogger<MusicProcessor> logger,
        IFileSystem fileSystem,
        IEnumerable<IMusicFileProcessor> fileProcessors)
    {
        this.logger = logger;
        this.fileSystem = fileSystem;
        this.fileProcessors = fileProcessors;
    }

    public void Process(string musicPath)
    {
        this.logger.LogInformation("Processing music directory: {MusicDir}", musicPath);

        if (!this.fileSystem.Directory.Exists(musicPath))
        {
            this.logger.LogWarning("The music directory does not exist: {MusicDir}", musicPath);
            return;
        }

        string[] artistDirs = this.fileSystem.Directory
            .GetDirectories(musicPath)
            .OrderBy(dir => dir, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        foreach (string artistPath in artistDirs)
        {
            this.ProcessArtist(artistPath);
        }
    }

    private void ProcessArtist(string artistPath)
    {
        Artist? artist = this.GetArtist(artistPath);
        if (artist == null)
        {
            return;
        }

        this.logger.LogInformation("Processing artist {ArtistName}: {ArtistPath}", artist.Name, artist.Path);

        string[] albumDirs = this.fileSystem.Directory
            .GetDirectories(artist.Path)
            .OrderBy(dir => dir, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        foreach (string albumPath in albumDirs)
        {
            this.ProcessAlbum(artist, albumPath);
        }
    }

    private void ProcessAlbum(Artist artist, string albumPath)
    {
        Album? album = this.GetAlbum(albumPath);
        if (album == null)
        {
            return;
        }

        this.logger.LogInformation("Processing album {ArtistName} - {AlbumName}: {AlbumPath}", artist.Name, album.Name, album.Path);

        List<string> discDirs = this.fileSystem.Directory
            .GetDirectories(album.Path)
            .OrderBy(dir => dir, StringComparer.OrdinalIgnoreCase)
            .ToList();

        discDirs = discDirs.Prepend(albumPath).ToList();

        for (uint disc = 0; disc < discDirs.Count; ++disc)
        {
            string discDir = discDirs[(int)disc];

            string[] musicFiles = this.fileSystem.Directory
                .GetFiles(discDir)
                .Where(file => SupportedExtensions.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase))
                .OrderBy(file => file, StringComparer.OrdinalIgnoreCase)
                .ToArray();

            foreach (string trackPath in musicFiles)
            {
                this.ProcessFile(artist, album, trackPath, disc > 0 ? disc : null);
            }
        }
    }

    private void ProcessFile(Artist artist, Album album, string filePath, uint? discNumber = null)
    {
        Track? track = this.GetTrack(filePath, discNumber);
        if (track == null)
        {
            return;
        }

        this.logger.LogInformation("Processing track {ArtistName} - {AlbumName} - {TrackName} : {AlbumPath}", artist.Name, album.Name, track.Name, track.Path);

        foreach (IMusicFileProcessor fileProcessor in this.fileProcessors)
        {
            fileProcessor.Process(artist, album, track);
        }
    }

    private Artist? GetArtist(string artistPath)
    {
        string? artistName = this.fileSystem.Path.GetFileName(artistPath);
        if (string.IsNullOrWhiteSpace(artistName))
        {
            return null;
        }

        return new Artist()
        {
            Path = artistPath,
            Name = artistName,
        };
    }

    private Album? GetAlbum(string albumPath)
    {
        string? albumDir = this.fileSystem.Path.GetFileName(albumPath);
        if (string.IsNullOrWhiteSpace(albumDir))
        {
            return null;
        }

        Match albumMatch = this.albumPattern.Match(albumDir);
        if (!albumMatch.Success)
        {
            return new Album()
            {
                Path = albumPath,
                Name = albumDir,
            };
        }

        return new Album()
        {
            Path = albumPath,
            Name = albumMatch.Groups["album"].Value,
            Year = uint.Parse(albumMatch.Groups["year"].Value),
        };
    }

    private Track? GetTrack(string filePath, uint? discNumber = null)
    {
        string? fileName = this.fileSystem.Path.GetFileNameWithoutExtension(filePath);
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return null;
        }

        Match trackMatch = this.trackPattern.Match(fileName);
        if (!trackMatch.Success)
        {
            return new Track()
            {
                Path = filePath,
                Name = fileName,
                Disc = discNumber,
            };
        }

        return new Track()
        {
            Path = filePath,
            Name = trackMatch.Groups["trackName"].Value,
            Number = uint.Parse(trackMatch.Groups["trackNumber"].Value),
            Disc = discNumber,
        };
    }
}
