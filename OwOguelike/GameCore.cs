using Chroma.Input;

namespace OwOguelike;

using Chroma;

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