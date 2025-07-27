namespace MusicManager.Core;

using System.Globalization;
using System.IO.Abstractions;
using MusicManager.Core.Files;

public class MusicFileFactory : IMusicFileFactory
{
    private readonly IFileSystem fileSystem;

    public MusicFileFactory(IFileSystem fileSystem)
    {
        this.fileSystem = fileSystem;
    }

    public IMusicFile? Load(string path)
    {
        string extension = this.fileSystem.Path.GetExtension(path);

        switch (extension.ToLower(CultureInfo.CurrentCulture))
        {
            case ".mp3":
                return new TagLibFile(path);
            default:
                return null;
        }
    }
}
