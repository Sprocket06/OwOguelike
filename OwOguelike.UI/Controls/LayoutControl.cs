namespace OwOguelike.UI;

public abstract class LayoutControl : Control
{
    public Rectangle Margins;

    public LayoutControl(int x = 0, int y = 0, int w = 0, int h = 0, Rectangle? margins = null)
        : base(x, y, w, h)
    {
        Margins = margins ?? Rectangle.Empty;
    }

    public LayoutControl(Vector2 pos, Size size, Rectangle? margins = null)
        : this((int)pos.X, (int)pos.Y, size.Width, size.Height, margins)
    {
    }

    public LayoutControl(Vector2 pos, int w = 0, int h = 0, Rectangle? margins = null)
        : this((int)pos.X, (int)pos.Y, w, h, margins)
    {
    }

    public LayoutControl(int x, int y, Size size, Rectangle? margins = null)
        : this(x, y, size.Width, size.Height, margins)
    {
    }

    public override void Draw(RenderContext context)
    {
        var offset = new Vector2(Margins.X, Margins.Y);
        RenderTransform.Translate(offset);
        base.Draw(context);
        RenderTransform.Translate(-offset);
    }
}