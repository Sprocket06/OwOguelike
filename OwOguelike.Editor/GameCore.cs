using Chroma;
using Chroma.Input;

namespace OwOguelike.Editor;

public class GameCore : Game
{
    public GameCore() : base(new(false, false))
    {
    }
    
    protected override void KeyPressed(KeyEventArgs e)
    {
        if (e.KeyCode == KeyCode.Escape)
        {
            Quit();
        }
    }
}