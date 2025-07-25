namespace MusicCleaner.Core;

using System.Globalization;
using System.IO.Abstractions;
using global::MusicCleaner.Core.Files;

public class MusicFileFactory : IMusicFileFactory
{
    private readonly IFileSystem fileSystem;

    public MusicFileFactory(IFileSystem fileSystem)
    {
        this.fileSystem = fileSystem;
    }

    public IMusicFile Load(string path)
    {
        string extension = this.fileSystem.Path.GetExtension(path);

#pragma warning disable CA1304 // Specify CultureInfo
        switch (extension.ToLower(CultureInfo.CurrentCulture))
        {
            case ".mp3":
                return new TagLibFile(path);
            default:
                throw new InvalidOperationException($"File type not supported: {extension}");
        }
#pragma warning restore CA1304 // Specify CultureInfo
    }
}
