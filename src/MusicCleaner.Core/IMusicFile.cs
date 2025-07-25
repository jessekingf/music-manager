namespace MusicCleaner.Core;

public interface IMusicFile : IDisposable
{
    string Path
    {
        get;
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

    void Save();
}
