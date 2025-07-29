namespace MusicManager.Core.Model;

public class Track
{
    public required string Path
    {
        get;
        init;
    }

    public required string Name
    {
        get;
        init;
    }

    public uint? TrackNumber
    {
        get;
        init;
    }

    public uint? DiscNumber
    {
        get;
        init;
    }
}
