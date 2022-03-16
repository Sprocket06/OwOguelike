namespace OwOguelike.UI.Controls;

public abstract class LayoutControl : Control
{
    public Border? Border { get; set; }
    public Rectangle Margins { get; set; }

    public LayoutControl(int x = 0, int y = 0, int w = 0, int h = 0) : base(x, y, w, h)
    {
    }

    public LayoutControl(Vector2 pos, Size size) : base(pos, size)
    {
    }

    public LayoutControl(Vector2 pos, int w = 0, int h = 0) : base(pos, w, h)
    {
    }

    public override void Draw(RenderContext context)
    {
        var offset = new Vector2(Margins.X, Margins.Y);
        RenderTransform.Translate(offset);
        base.Draw(context);
        RenderTransform.Translate(-offset);
        if (Border is not null)
        {
            var oldL = RenderSettings.LineThickness;
            RenderSettings.LineThickness = Border.LineThickness;
            if (Border.Radius > 0)
                context.RoundedRect(ShapeMode.Stroke, Position, Size, Border.Radius, Border.Color);
            else
                context.Rectangle(ShapeMode.Stroke, X, Y, Width, Height, Border.Color);
            RenderSettings.LineThickness = oldL;
        }
    }
}