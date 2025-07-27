namespace MusicManager.Core.Model;

public record Album
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

    public int? Year
    {
        get;
        init;
    }
}
