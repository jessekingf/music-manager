namespace MusicCleaner.Exceptions;

internal class InvalidOptionException : Exception
{
    public InvalidOptionException()
    {
    }

    public InvalidOptionException(string message)
        : base(message)
    {
    }

    public InvalidOptionException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
