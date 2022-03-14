using System.Drawing;
using System.Numerics;
using Chroma;
using Chroma.Diagnostics;
using Chroma.Graphics;
using Color = Chroma.Graphics.Color;

namespace OwOguelike.UI.Tests;

public class GameCore : Game
{
    public Panel _panel;
    
    public GameCore() : base(new(false, false))
    {
        Graphics.VerticalSyncMode = VerticalSyncMode.None;
        Graphics.LimitFramerate = false;
        
        _panel = new Panel(100, 100, 100, 100, Rectangle.FromLTRB(4, 4, 4, 4));
        _panel.AddChild(new Button("owjdnufnwqnfwnfpqwfnwqpofnqwpofqwnfpoquiwfuwoqfhuwhfuwqhfuwqf"));
        FixedTimeStepTarget = 1;
    }

    protected override void Draw(RenderContext context)
    {
        context.Clear(Color.Green);
        _panel.Draw(context);
        context.DrawString("test", Vector2.Zero, Color.Red);
    }

    protected override void FixedUpdate(float delta)
    {
        Window.Title = PerformanceCounter.FPS.ToString();
    }
}