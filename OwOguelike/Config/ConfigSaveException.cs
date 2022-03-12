namespace OwOguelike.Config;

public class ConfigSaveException : Exception
{
    public ConfigSaveException()
    {
    }

    public ConfigSaveException(string? message) : base(message)
    {
    }

    public ConfigSaveException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}