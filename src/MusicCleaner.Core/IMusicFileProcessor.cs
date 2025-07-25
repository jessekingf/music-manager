namespace MusicCleaner.Core;

public interface IMusicFileProcessor
{
    string Process(string artistName, string albumName, string filePath);
}
