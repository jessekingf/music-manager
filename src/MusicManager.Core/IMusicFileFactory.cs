namespace MusicManager.Core;

public interface IMusicFileFactory
{
    IMusicFile Load(string path);
}
