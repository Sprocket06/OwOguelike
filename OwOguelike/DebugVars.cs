namespace OwOguelike;

public class DebugVars
{
    // 0 = Show nothing
    // 1 = Show FPS
    // 2 = Show Scene
    // 3 = Show Level
    [ConVar("cl_showfps")]
    public static int ShowFPS = 0;
    
    [ConVar("sv_cheats")] 
    private static bool Cheats = false;
}