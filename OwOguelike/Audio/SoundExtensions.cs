namespace OwOguelike.Audio;

public static class SoundExtensions
{
    public static void UpdatePanning(this Sound self, Vector2 sourcePosition, Vector2 listenerPosition)
    {
        var normalized = -((listenerPosition - sourcePosition) / listenerPosition);
        self.Panning = normalized.X;
    }

    public static void UpdatePanning(this Sound self, float sourcePosition, float listenerPosition) =>
        UpdatePanning(self, new Vector2(sourcePosition), new Vector2(listenerPosition));
    
    public static void UpdatePanning(this Sound self, Vector2 sourcePosition, float listenerPosition) =>
        UpdatePanning(self, sourcePosition, new Vector2(listenerPosition));
    
    public static void UpdatePanning(this Sound self, float sourcePosition, Vector2 listenerPosition) =>
        UpdatePanning(self, new Vector2(sourcePosition), listenerPosition);
}