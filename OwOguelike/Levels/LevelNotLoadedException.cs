namespace OwOguelike.Levels;

public class LevelNotLoadedException : Exception
{
    public LevelNotLoadedException()
    {
    }

    public LevelNotLoadedException(string? message) : base(message)
    {
    }
}