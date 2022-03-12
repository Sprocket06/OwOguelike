namespace OwOguelike.Audio;

// TODO: Make an AudioSource that actually fully loads the waveform into memory and make this load those instead
// Music should be loaded as the normal Music class because that is streamed in realtime as it should be
public static partial class Sfx
{
    public static readonly Dictionary<string, Sound?> LoadedClips;

    static Sfx()
    {
        LoadedClips = new(_builtinClips.Count);
    }

    // TODO: This assumes that the gamecore is ready to load content! Maybe its not! Idk!
    public static Sound LoadClip(string path)
    {
        if (LoadedClips.ContainsKey(path))
            return LoadedClips[path]!;

        try
        {
            var clip = ContentProvider!.Load<Sound>(path);
            LoadedClips.Add(path, clip);
            return clip;
        }
        catch (Exception e)
        {
            // Not our problem
            throw new Exception($"Failed to load clip: {path}", e);
        }
    }

    public static void LoadClip(AudioClip clip)
    {
        if (_builtinClips.ContainsKey(clip))
            LoadClip(_builtinClips[clip]);
    }

    [ConsoleCommand("stopsounds")]
    public static void StopAllSounds()
    {
        foreach (var sound in LoadedClips.Values)
        {
            sound?.Stop();
        }
    }

    public static Sound PlayClip(Sound sound)
    {
        sound.Play();
        return sound;
    }

    public static Sound PlayClip(Sound sound, Vector2 sourcePosition, Vector2 listenerPosition)
    {
        sound.UpdatePanning(sourcePosition, listenerPosition);
        sound.Play();
        return sound;
    }

    [ConsoleCommand("playsound")]
    public static Sound PlayClip(string path) => PlayClip(LoadClip(path));

    public static Sound PlayClip(string path, Vector2 sourcePosition, Vector2 listenerPosition) 
        => PlayClip(LoadClip(path), sourcePosition, listenerPosition);

    [ConsoleCommand("soundtest")]
    public static Sound? PlayClip(AudioClip clip)
    {
        if (_builtinClips.ContainsKey(clip))
            return PlayClip(_builtinClips[clip]);

        return null;
    }

    public static Sound? PlayClip(AudioClip clip, Vector2 sourcePosition, Vector2 listenerPosition)
    {
        if (_builtinClips.ContainsKey(clip))
            return PlayClip(_builtinClips[clip], sourcePosition, listenerPosition);

        return null;
    }
}