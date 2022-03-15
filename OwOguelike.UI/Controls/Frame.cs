namespace OwOguelike.UI;

public class Frame : LayoutControl
{
    public Frame(int x = 0, int y = 0, int w = 0, int h = 0, Rectangle? margins = null) : base(x, y, w, h, margins)
    {
    }

    public Frame(Vector2 pos, Size size, Rectangle? margins = null) : base(pos, size, margins)
    {
    }

    public Frame(Vector2 pos, int w = 0, int h = 0, Rectangle? margins = null) : base(pos, w, h, margins)
    {
    }

    public Frame(int x, int y, Size size, Rectangle? margins = null) : base(x, y, size, margins)
    {
    }

    public override void Draw(RenderContext context)
    {
        var old = RenderSettings.Scissor;
        RenderSettings.Scissor = new(
            X + Margins.Left,
            Y + Margins.Top,
            Width - Margins.Right - Margins.Left,
            Height - Margins.Bottom - Margins.Top
        );
        base.Draw(context);
        RenderSettings.Scissor = old;
    }
}