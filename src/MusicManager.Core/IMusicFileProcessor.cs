namespace MusicManager.Core;

using MusicManager.Core.Model;

public interface IMusicFileProcessor
{
    void Process(Artist artist, Album album, Track track);
}
