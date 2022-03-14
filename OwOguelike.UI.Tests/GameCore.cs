using System.Drawing;
using Chroma;
using Chroma.Graphics;

namespace OwOguelike.UI.Tests;

public class GameCore : Game
{
    public Panel _panel;
    
    public GameCore() : base(new(false, false))
    {
        _panel = new Panel(100, 100, 100, 100, Rectangle.FromLTRB(4, 4, 4, 4));
        _panel.AddChild(new Button("owjdnufnwqnfwnfpqwfnwqpofnqwpofqwnfpoquiwfuwoqfhuwhfuwqhfuwqf"));
    }

    protected override void Draw(RenderContext context)
    {
        _panel.Draw(context);
    }
}