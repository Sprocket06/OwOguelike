namespace OwOguelike.Editor;

using Chroma;
using Chroma.Input;

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