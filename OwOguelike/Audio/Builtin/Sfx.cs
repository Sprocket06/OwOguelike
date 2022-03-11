namespace OwOguelike.Audio;

public static partial class Sfx
{
    private static readonly Dictionary<AudioClip, string> _builtinClips = new()
    {
        { AudioClip.GunFireGeneric, "gunfire.ogg" },
        { AudioClip.GunReloadGeneric, "reload.ogg" },
        { AudioClip.FootstepGeneric, "steps.ogg" },
        { AudioClip.DashSound, "dash.ogg" },
    };
}