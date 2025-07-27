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

    public int? Number
    {
        get;
        init;
    }
}
