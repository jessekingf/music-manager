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

        if (string.IsNullOrEmpty(musicFile.Artist)
            || musicFile.Artist.Contains("Unknown artist", StringComparison.CurrentCultureIgnoreCase))
        {
            musicFile.Artist = artist.Name;
        }

        if (musicFile.AlbumArtist != artist.Name)
        {
            musicFile.AlbumArtist = artist.Name;
        }

        if (string.IsNullOrEmpty(musicFile.Album)
            || musicFile.Album.Contains("Unknown album", StringComparison.CurrentCultureIgnoreCase))
        {
            musicFile.Album = album.Name;
        }

        if ((musicFile.Year == null || musicFile.Year == 0)
            && album.Year != null && album.Year > 0)
        {
            musicFile.Year = album.Year;
        }

        if (string.IsNullOrEmpty(musicFile.Title)
            || musicFile.Title.Contains("Track 1", StringComparison.CurrentCultureIgnoreCase))
        {
            musicFile.Album = track.Name;
        }

        if ((musicFile.Track == null || musicFile.Track == 0)
            && track.Number != null && track.Number > 0)
        {
            musicFile.Track = track.Number;
        }

        if (track.Disc > 0 && musicFile.Disc != track.Disc)
        {
            musicFile.Disc = track.Disc;
        }

        if (!string.IsNullOrEmpty(musicFile.Genre)
            && musicFile.Genre.Contains("Unknown", StringComparison.CurrentCultureIgnoreCase))
        {
            musicFile.Genre = null;
        }

        if (!string.IsNullOrEmpty(musicFile.Comment))
        {
            musicFile.Comment = null;
        }

        musicFile.Save();
    }
}
