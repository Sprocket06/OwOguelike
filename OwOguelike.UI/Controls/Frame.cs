namespace OwOguelike.UI.Controls;

public class Frame : LayoutControl
{
    public Frame(int x = 0, int y = 0, int w = 0, int h = 0) : base(x, y, w, h)
    {
    }

    public Frame(Vector2 pos, Size size) : base(pos, size)
    {
    }

    public Frame(Vector2 pos, int w = 0, int h = 0) : base(pos, w, h)
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