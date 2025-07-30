namespace MusicManager.Core.Files;

using TagLib;

public class TagLibFile : IMusicFile
{
    private readonly File file;
    private bool isDisposed;

    public TagLibFile(string path)
    {
        this.file = File.Create(path);

        Tag tag = this.file.Tag;
        this.Artist = string.Join(", ", tag.Performers);
        this.AlbumArtist = string.Join(", ", tag.AlbumArtists);
        this.Album = tag.Album;
        this.Title = tag.Title;
        this.Track = tag.Track > 0 ? tag.Track : null;
        this.Disc = tag.Disc > 0 ? tag.Disc : null;
        this.Year = tag.Year > 0 ? tag.Year : null;
        this.Comment = tag.Comment;
        this.Genre = string.Join(", ", tag.Genres);
    }

    public string Path
    {
        get
        {
            return this.file.Name;
        }
    }

    public string? Artist
    {
        get;
        set;
    }

    public string? AlbumArtist
    {
        get;
        set;
    }

    public string? Album
    {
        get;
        set;
    }

    public string? Title
    {
        get;
        set;
    }

    public uint? Track
    {
        get;
        set;
    }

    public uint? Disc
    {
        get;
        set;
    }

    public uint? Year
    {
        get;
        set;
    }

    public string? Comment
    {
        get;
        set;
    }

    public string? Genre
    {
        get;
        set;
    }

    public void Save()
    {
        Tag tag = this.file.Tag;

        tag.Performers = [this.Artist ?? string.Empty];
        tag.AlbumArtists = [this.AlbumArtist ?? string.Empty];
        tag.Album = this.Album ?? string.Empty;
        tag.Title = this.Title ?? string.Empty;
        tag.Track = this.Track ?? 0;
        tag.Disc = this.Disc ?? 0;
        tag.Year = this.Year ?? 0;
        tag.Comment = this.Comment ?? string.Empty;
        tag.Genres = [this.Genre ?? string.Empty];

        this.file.Save();
    }

    public void Dispose()
    {
        this.Dispose(isDisposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool isDisposing)
    {
        if (this.isDisposed)
        {
            return;
        }

        if (isDisposing)
        {
            this.file.Dispose();
        }

        this.isDisposed = true;
    }
}
