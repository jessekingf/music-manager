namespace MusicCleaner.Core;

public interface IMusicFileFactory
{
    IMusicFile Load(string path);
}
